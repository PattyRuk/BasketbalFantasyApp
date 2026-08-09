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


    }
}
