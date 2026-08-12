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



            return RedirectToAction("Summary", "TournamentGameplay", new { id = tournament.TournamentId });
        }
    }
}