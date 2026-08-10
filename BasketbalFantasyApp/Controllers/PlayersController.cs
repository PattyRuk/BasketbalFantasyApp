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
    }
}
