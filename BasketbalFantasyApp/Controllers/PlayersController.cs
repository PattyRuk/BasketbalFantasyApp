using BasketbalFantasyApp.DAL;
using BasketbalFantasyApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BasketbalFantasyApp.Controllers
{
    [Authorize]
    public class PlayersController : Controller
    {
        private readonly BasketbalFantasyDbContext _database;

        public PlayersController(BasketbalFantasyDbContext database)
        {
            _database = database;
        }

        public async Task<IActionResult> Index()
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var userTeam = await _database.Teams.FirstOrDefaultAsync(t => t.OwnerUserId == currentUserId);

            if (userTeam == null) return RedirectToAction("Create", "Teams");

            // Pull only the players drafted by this specific manager
            var rosterList = await _database.Players
                .Where(player => player.TeamId == userTeam.TeamId && player.OwnerUserId == currentUserId)
                .ToListAsync();

            return View(rosterList);
        }

        [AllowAnonymous]
        public async Task<IActionResult> AvailablePlayers()
        {
            var poolViewModel = new AvailablePlayersViewModel();

            // Fetch players from (the System Pool) haven't been drafted yet
            poolViewModel.AvailablePlayers = await _database.Players
                .Include(p => p.Stats)
                .Where(p => p.OwnerUserId == "SYSTEM_POOL" || p.TeamId == 1)
                .OrderBy(p => p.LastName)
                .ToListAsync();

            return View(poolViewModel);
        }

        // CREATE
        public async Task<IActionResult> Create()
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var userTeam = await _database.Teams.FirstOrDefaultAsync(t => t.OwnerUserId == currentUserId);

            if (userTeam == null) return RedirectToAction("Create", "Teams");

            // Find all players still sitting in the unassigned pool (TeamId = 1 or OwnerUserId = 'SYSTEM_POOL')
            var availablePool = await _database.Players
                .Where(player => player.OwnerUserId == "SYSTEM_POOL" || player.TeamId == 1)
                .Select(player => new {
                    Id = player.Id,
                    DisplayText = $"{player.FirstName} {player.LastName} ({player.Position} - {player.NbaTeam})"
                })
                .ToListAsync();

            ViewBag.TeamId = userTeam.TeamId;
            ViewBag.PlayerDropdown = new SelectList(availablePool, "Id", "DisplayText");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int selectedPlayerId)
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var userTeam = await _database.Teams.FirstOrDefaultAsync(t => t.OwnerUserId == currentUserId);

            if (userTeam == null) return RedirectToAction("Create", "Teams");

            var playerToDraft = await _database.Players.FindAsync(selectedPlayerId);

            if (playerToDraft != null)
            {
                // Update properties to assign ownership to this manager's custom team franchise
                playerToDraft.TeamId = userTeam.TeamId;
                playerToDraft.OwnerUserId = currentUserId;

                _database.Update(playerToDraft);
                await _database.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        // EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var playerRecord = await _database.Players.FindAsync(id);
            if (playerRecord == null) return NotFound();

            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var userTeam = await _database.Teams.FirstOrDefaultAsync(t => t.OwnerUserId == currentUserId);

            if (userTeam == null || playerRecord.TeamId != userTeam.TeamId) return Forbid();

            return View(playerRecord);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Player modifiedPlayer)
        {
            if (id != modifiedPlayer.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _database.Update(modifiedPlayer);
                await _database.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(modifiedPlayer);
        }

        // DELETE
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var playerRecord = await _database.Players
                .Include(p => p.Team)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (playerRecord == null) return NotFound();

            return View(playerRecord);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var playerRecord = await _database.Players.FindAsync(id);
            if (playerRecord != null)
            {
                // when a manager deletes a player from their team, player released back into the public selection pool
                playerRecord.TeamId = 1;
                playerRecord.OwnerUserId = "SYSTEM_POOL";

                _database.Update(playerRecord);
                await _database.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        [AllowAnonymous]
        public async Task<IActionResult> CareerSummary(int id)
        {
            var player = await _database.Players
                .Include(p => p.Team)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (player == null) return NotFound();

            var statsLogs = await _database.PlayerStats
                .Include(ps => ps.Tournament)
                .Where(ps => ps.PlayerId == id)
                .ToListAsync();

            var viewModel = new PlayerSummaryViewModel
            {
                PlayerBio = player,
                DraftedTeam = player.Team ?? new Team { TeamName = "Unassigned Free Agent Pool" },
                TotalGamesPlayed = statsLogs.Count,
                TotalPointsScored = statsLogs.Sum(s => s.Points),
                AveragePointsPerGame = statsLogs.Any() ? System.Math.Round(statsLogs.Average(s => s.Points), 1) : 0,
                AverageRebounds = statsLogs.Any() ? System.Math.Round(statsLogs.Average(s => s.Rebounds), 1) : 0,
                AverageAssists = statsLogs.Any() ? System.Math.Round(statsLogs.Average(s => s.Assists), 1) : 0
            };

            // Calculate details per separate tournament block context
            viewModel.PerformanceHistory = statsLogs
                .Where(s => s.TournamentId.HasValue)
                .GroupBy(s => s.TournamentId)
                .Select(g => new TournamentStatsRow
                {
                    TournamentName = g.First().Tournament?.TournamentName ?? "League Exhibition Match",
                    GamesPlayed = g.Count(),
                    PointsScored = g.Sum(s => s.Points),
                    FieldGoalPercentage = System.Math.Round(g.Average(s => s.FieldGoalPercentage), 3)
                })
                .ToList();

            return View(viewModel);
        }
    }
}
