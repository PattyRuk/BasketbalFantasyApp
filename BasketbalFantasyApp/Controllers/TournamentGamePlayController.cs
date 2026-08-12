using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BasketbalFantasyApp.DAL;
using BasketbalFantasyApp.Models;

namespace BasketbalFantasyApp.Controllers
{
    [Authorize]
    public class TournamentGameplayController : Controller
    {
        private readonly BasketbalFantasyDbContext _database;

        public TournamentGameplayController(BasketbalFantasyDbContext database)
        {
            _database = database;
        }

        // Register Team
        public async Task<IActionResult> RegisterTeam(int id)
        {
            var tournament = await _database.Tournaments.FindAsync(id);
            if (tournament == null || tournament.IsCompleted) return NotFound();

            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var userTeam = await _database.Teams.FirstOrDefaultAsync(t => t.OwnerUserId == currentUserId);
            if (userTeam == null) return RedirectToAction("Create", "Teams");

            int rulesSize = tournament.FormatType.Contains("3x3") ? 3 : 5;

            var currentRoster = await _database.Players
                .Where(p => p.TeamId == userTeam.TeamId && p.OwnerUserId == currentUserId)
                .ToListAsync();

            var registrationViewModel = new TournamentRegisterViewModel
            {
                TournamentId = tournament.TournamentId,
                TournamentName = tournament.TournamentName,
                FormatType = tournament.FormatType,
                RequiredPlayersCount = rulesSize,
                EligibleRoster = currentRoster
            };

            return View(registrationViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterTeam(int tournamentId, List<int> selectedPlayerIds)
        {
            var tournament = await _database.Tournaments.FindAsync(tournamentId);
            if (tournament == null || tournament.IsCompleted) return NotFound();

            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var userTeam = await _database.Teams.FirstOrDefaultAsync(t => t.OwnerUserId == currentUserId);
            if (userTeam == null) return BadRequest();

            int rulesSize = tournament.FormatType.Contains("3x3") ? 3 : 5;
            if (selectedPlayerIds == null || selectedPlayerIds.Count != rulesSize)
            {
                return RedirectToAction(nameof(RegisterTeam), new { id = tournamentId });
            }

            bool alreadyRegistered = await _database.TournamentTeams
                .AnyAsync(tt => tt.TournamentId == tournamentId && tt.TeamId == userTeam.TeamId);
            if (alreadyRegistered) return RedirectToAction("Index", "Tournaments");

            var tournamentRegistration = new TournamentTeam
            {
                TournamentId = tournamentId,
                TeamId = userTeam.TeamId,
                FinalPosition = "Contender"
            };

            var players = await _database.Players.Where(p => selectedPlayerIds.Contains(p.Id)).ToListAsync();
            tournamentRegistration.RegisteredPlayers.AddRange(players);

            _database.TournamentTeams.Add(tournamentRegistration);
            await _database.SaveChangesAsync();

            return RedirectToAction("Index", "Tournaments");
        }

        // Tournament Summary Display
        [AllowAnonymous]
        public async Task<IActionResult> Summary(int id)
        {
            var tournament = await _database.Tournaments
                .Include(t => t.WinnerTeam)
                .Include(t => t.MvpPlayer)
                .Include(t => t.Games)
                .FirstOrDefaultAsync(t => t.TournamentId == id);

            if (tournament == null || !tournament.IsCompleted) return RedirectToAction("Index", "Tournaments");

            var standings = await _database.TournamentTeams
                .Include(tt => tt.Team)
                .Where(tt => tt.TournamentId == id)
                .OrderByDescending(tt => tt.WinsCount)
                .ToListAsync();

            var winnerRoster = await _database.TournamentTeams
                .Include(tt => tt.RegisteredPlayers)
                .FirstOrDefaultAsync(tt => tt.TournamentId == id && tt.TeamId == tournament.WinnerTeamId);

            int totalMvpPoints = 0;
            double avgMvpPoints = 0;
            if (tournament.MvpPlayerId.HasValue)
            {
                var mvpStats = await _database.PlayerStats
                    .Where(ps => ps.PlayerId == tournament.MvpPlayerId.Value && ps.GameDate >= tournament.EventDate)
                    .ToListAsync();
                if (mvpStats.Any())
                {
                    totalMvpPoints = mvpStats.Sum(s => s.Points);
                    avgMvpPoints = System.Math.Round(mvpStats.Average(s => s.Points), 1);
                }
            }

            var summaryModel = new TournamentSummaryViewModel
            {
                TournamentDetails = tournament,
                Standings = standings,
                WinnerTeam = tournament.WinnerTeam ?? new Team(),
                WinnerRoster = winnerRoster?.RegisteredPlayers ?? new List<Player>(),
                TournamentMvp = tournament.MvpPlayer ?? new Player(),
                MvpTotalPoints = totalMvpPoints,
                MvpAveragePoints = avgMvpPoints
            };

            return View(summaryModel);
        }
        //Anyone can view individual match results 
        [AllowAnonymous]
        public async Task<IActionResult> GameLogs(int id)
        {
            var tournament = await _database.Tournaments.FindAsync(id);
            if (tournament == null) return NotFound();

            var gamesList = await _database.Games
                .Include(g => g.TeamA)
                .Include(g => g.TeamB)
                .Where(g => g.TournamentId == id)
                .OrderByDescending(g => g.MatchTimestamp)
                .ToListAsync();

            var viewModel = new TournamentGamesViewModel
            {
                TournamentDetails = tournament,
                PlayedGames = gamesList
            };

            return View(viewModel);
        }
    }
}
