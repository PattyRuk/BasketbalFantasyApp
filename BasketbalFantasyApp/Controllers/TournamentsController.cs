using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BasketbalFantasyApp.DAL;
using BasketbalFantasyApp.Models;

namespace BasketbalFantasyApp.Controllers
{
    [Authorize]
    public class TournamentsController : Controller
    {
        private readonly BasketbalFantasyDbContext _database;

        public TournamentsController(BasketbalFantasyDbContext database)
        {
            _database = database;
        }

        // READ
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var brackets = await _database.Tournaments
                .Include(t => t.TournamentTeams).ThenInclude(tt => tt.Team)
                .Include(t => t.WinnerTeam)
                .OrderByDescending(t => t.EventDate)
                .ToListAsync();

            return View(brackets);
        }

        // CREATE
        [Authorize(Roles = "Admin")]
        public IActionResult Create()  
        {
            var defaultTournament = new Tournament { EventDate = DateTime.Today.AddDays(1) };
            return View(defaultTournament);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tournament newTournament)
        {
            if (ModelState.IsValid)
            {
                _database.Add(newTournament);
                await _database.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(newTournament);
        }

        // DELETE
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var targetTournament = await _database.Tournaments.FindAsync(id);
            if (targetTournament != null)
            {
                _database.Tournaments.Remove(targetTournament);
                await _database.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
