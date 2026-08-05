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

    }
}
