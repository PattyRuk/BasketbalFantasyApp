using BasketbalFantasyApp.DAL;
using BasketbalFantasyApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userTeam = await _database.Teams.FirstOrDefaultAsync(t => t.OwnerUserId == currentUserId);

            if (userTeam == null)
            {
                return RedirectToAction("CreateTeam", "Teams");
            }

            var rosterList = await _database.Players
                .Where(player => player.TeamId == userTeam.TeamId)
                .ToListAsync();

            return View(rosterList);
        }

        public async Task<IActionResult> Create()
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userTeam = await _database.Teams.FirstOrDefaultAsync(t => t.OwnerUserId == currentUserId);

            if (userTeam == null) return RedirectToAction("CreateTeam", "Teams");

            ViewBag.TeamId = userTeam.TeamId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Player newPlayer)
        {
            if (ModelState.IsValid)
            {
                _database.Add(newPlayer);
                await _database.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(newPlayer);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var playerRecord = await _database.Players.FindAsync(id);
            if (playerRecord == null) return NotFound();

            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userTeam = await _database.Teams.FirstOrDefaultAsync(t => t.OwnerUserId == currentUserId);

            if (userTeam == null || playerRecord.TeamId != userTeam.TeamId)
            {
                return Forbid();
            }

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
                _database.Players.Remove(playerRecord);
                await _database.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
