using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BasketbalFantasyApp.DAL;
using BasketbalFantasyApp.Models;

namespace BasketbalFantasyApp.Controllers
{
    public class TournamentsController : Controller
    {
        private readonly BasketbalFantasyDbContext _database;

        public TournamentsController(BasketbalFantasyDbContext database)
        {
            _database = database;
        }

        [AllowAnonymous] // Anyone can view this list
        public async Task<IActionResult> Index()
        {
            var scheduledBrackets = await _database.Tournaments
                .OrderBy(tournament => tournament.EventDate)
                .ToListAsync();

            return View(scheduledBrackets);
        }


        [Authorize(Roles = "Admin")] // Blocks standard users and anonymous visitors
        public IActionResult Create()
        {
            // default calendar date to tomorrow
            var tomorrow = DateTime.Today.AddDays(1);
            var defaultTournament = new Tournament { EventDate = tomorrow };

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

                // Route back to the brackets schedule upon success
                return RedirectToAction(nameof(Index));
            }

            return View(newTournament);
        }

    }
}
