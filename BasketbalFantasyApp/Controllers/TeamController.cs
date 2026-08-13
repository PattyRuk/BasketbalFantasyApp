using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BasketbalFantasyApp.DAL;
using BasketbalFantasyApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BasketbalFantasyApp.Controllers
{
    [Authorize] // Must be logged in to manage a team
    public class TeamsController : Controller
    {
        private readonly BasketbalFantasyDbContext _database;

        public TeamsController(BasketbalFantasyDbContext database)
        {
            _database = database;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var leagueTeams = await _database.Teams
                .Include(t => t.Players)
                .ToListAsync();

            return View(leagueTeams);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Team newTeam)
        {
            // Get unique Account User ID of the currently logged-in manager/user
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Assign the logged-in user as the permanent team owner
            newTeam.OwnerUserId = currentUserId;

            if (ModelState.IsValid)
            {
                _database.Add(newTeam);
                await _database.SaveChangesAsync();

                // Route them right back to the Home Dashboard upon success
                return RedirectToAction("Index", "Home");
            }

            return View(newTeam);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var teamRecord = await _database.Teams
                .Include(t => t.Players)
                .FirstOrDefaultAsync(t => t.TeamId == id);

            if (teamRecord == null) return NotFound();

            return View(teamRecord);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teamRecord = await _database.Teams.FindAsync(id);
            if (teamRecord == null) return RedirectToAction(nameof(Index));

            // REMOVE TOURNAMENT REGISTRATIONS TIED TO THIS FRANCHISE ID
            var tournamentRegistrations = await _database.TournamentTeams
                .Include(tt => tt.RegisteredPlayers)
                .Where(tt => tt.TeamId == id)
                .ToListAsync();

            foreach (var tr in tournamentRegistrations)
            {
                tr.RegisteredPlayers.Clear();
            }
            await _database.SaveChangesAsync();

            if (tournamentRegistrations.Any())
            {
                _database.TournamentTeams.RemoveRange(tournamentRegistrations);
            }

            // RELEASE TEAM PLAYERS 
            var activeRosterPlayers = await _database.Players.Where(p => p.TeamId == id).ToListAsync();
            foreach (var player in activeRosterPlayers)
            {
                player.TeamId = 1; // Return back to Global Player Pool
                player.OwnerUserId = "SYSTEM_POOL";
                _database.Update(player);
            }

            _database.Teams.Remove(teamRecord);
            await _database.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}