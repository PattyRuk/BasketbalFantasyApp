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



            return RedirectToAction("Summary", "TournamentGameplay", new { id = tournament.TournamentId });
        }
    }
}