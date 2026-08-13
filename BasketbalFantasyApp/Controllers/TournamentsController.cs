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
        
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var targetTournament = await _database.Tournaments.FindAsync(id);
            if (targetTournament == null) return RedirectToAction(nameof(Index));

            // REMOVE PLAYER BOX SCORE STATS LINKED TO THIS TOURNAMENT 
            var linkedStats = await _database.PlayerStats.Where(ps => ps.TournamentId == id).ToListAsync();
            if (linkedStats.Any())
            {
                _database.PlayerStats.RemoveRange(linkedStats);
            }

            // REMOVE THE INDIVIDUAL GAME SCORE SHEETS FOR THIS TOURNAMENT
            var linkedGames = await _database.Games.Where(g => g.TournamentId == id).ToListAsync();
            if (linkedGames.Any())
            {
                _database.Games.RemoveRange(linkedGames);
            }

            // REMOVE TOURNAMENT TEAM REGISTRATIONS
            var linkedTournamentTeams = await _database.TournamentTeams
                   .Include(tt => tt.RegisteredPlayers)
                   .Where(tt => tt.TournamentId == id)
                   .ToListAsync();
            foreach (var tt in linkedTournamentTeams)
            {
                tt.RegisteredPlayers.Clear(); // Clear the collection to break the shadow foreign key pointer constraints on the Players table
            }
            await _database.SaveChangesAsync();

            if (linkedTournamentTeams.Any())
            {
                _database.TournamentTeams.RemoveRange(linkedTournamentTeams);
            }
            _database.Tournaments.Remove(targetTournament);
            await _database.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
