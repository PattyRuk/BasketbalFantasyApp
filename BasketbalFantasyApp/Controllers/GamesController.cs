using BasketbalFantasyApp.DAL;
using BasketbalFantasyApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BasketbalFantasyApp.Controllers
{
    [Authorize(Roles = "Admin")] // Locked entirely to League Administrators
    public class GamesController : Controller
    {
        private readonly BasketbalFantasyDbContext _database;

        public GamesController(BasketbalFantasyDbContext database)
        {
            _database = database;
        }

        // Run Matchups & Generate Player Stats
        public async Task<IActionResult> SimulateTournament(int id)
        {
            var tournament = await _database.Tournaments
                .Include(t => t.TournamentTeams).ThenInclude(tt => tt.RegisteredPlayers)
                .FirstOrDefaultAsync(t => t.TournamentId == id);

            if (tournament == null || tournament.IsCompleted || tournament.TournamentTeams.Count < 2)
            {
                return RedirectToAction("Index", "Tournaments");
            }

            var random = new Random();

            // Run matchup loops until a contender team hits the win threshold
            while (!tournament.IsCompleted)
            {
                var contenders = tournament.TournamentTeams.Where(tt => tt.FinalPosition == "Contender").ToList();
                if (contenders.Count < 2) break;

                //SEPARATE AND EXTRACT TWO DISTINCT TEAMS FROM THE POOL LIST
                var teamA = contenders[random.Next(contenders.Count)];
                var teamB = contenders.Where(t => t.TeamId != teamA.TeamId).ToList()[random.Next(contenders.Count - 1)];

                int scoreA = random.Next(70, 115);
                int scoreB = random.Next(70, 115);
                while (scoreA == scoreB) scoreB = random.Next(70, 115);

                var matchLog = new Game
                {
                    TournamentId = tournament.TournamentId,
                    TeamAId = teamA.TeamId,
                    TeamBId = teamB.TeamId,
                    TeamAScore = scoreA,
                    TeamBScore = scoreB,
                    MatchTimestamp = DateTime.Now
                };

                var winner = scoreA > scoreB ? teamA : teamB;
                var loser = scoreA > scoreB ? teamB : teamA;

                matchLog.WinnerTeamId = winner.TeamId;
                _database.Games.Add(matchLog);

                winner.WinsCount++;
                loser.LossesCount++;

                // Generate Realistic Stats Per Athlete for the game
                var activeRosterList = winner.RegisteredPlayers.Concat(loser.RegisteredPlayers).ToList();
                foreach (var player in activeRosterList)
                {
                    _database.PlayerStats.Add(new PlayerStats
                    {
                        PlayerId = player.Id,
                        GameDate = DateTime.Now,
                        Points = random.Next(8, 38),
                        Rebounds = random.Next(2, 15),
                        Assists = random.Next(1, 12),
                        Steals = random.Next(0, 4),
                        Blocks = random.Next(0, 5),
                        Turnovers = random.Next(1, 5),
                        ThreePointersMade = random.Next(0, 6),
                        FieldGoalPercentage = Math.Round(random.NextDouble() * (0.65 - 0.35) + 0.35, 3),
                        FreeThrowPercentage = Math.Round(random.NextDouble() * (0.95 - 0.55) + 0.55, 3)
                    });
                }

                if (winner.WinsCount >= tournament.RequiredWins)
                {
                    tournament.IsCompleted = true;
                    tournament.WinnerTeamId = winner.TeamId;
                    winner.FinalPosition = "Champion";
                    loser.FinalPosition = "Runner-Up";

                    foreach (var remaining in tournament.TournamentTeams.Where(tt => tt.FinalPosition == "Contender"))
                    {
                        remaining.FinalPosition = "Eliminated";
                    }
                }

                await _database.SaveChangesAsync();
            }

            // Calculate MVP by tracking the athlete with the highest points sum
            if (tournament.IsCompleted)
            {
                var registeredPlayerIds = tournament.TournamentTeams.SelectMany(tt => tt.RegisteredPlayers.Select(p => p.Id)).ToList();

                var mvpCandidate = await _database.PlayerStats
                    .Where(ps => registeredPlayerIds.Contains(ps.PlayerId) && ps.GameDate >= tournament.EventDate)
                    .GroupBy(ps => ps.PlayerId)
                    .Select(g => new { PlayerId = g.Key, TotalPoints = g.Sum(ps => ps.Points) })
                    .OrderByDescending(x => x.TotalPoints)
                    .FirstOrDefaultAsync();

                if (mvpCandidate != null)
                {
                    tournament.MvpPlayerId = mvpCandidate.PlayerId;
                    await _database.SaveChangesAsync();
                }
            }

            return RedirectToAction("Summary", "TournamentGameplay", new { id = tournament.TournamentId });
        }
    }
}