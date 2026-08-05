using BasketbalFantasyApp.DAL;
using BasketbalFantasyApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace BasketbalFantasyApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly BasketbalFantasyDbContext _database;

        public HomeController(BasketbalFantasyDbContext database)
        {
            _database = database;
        }

        public async Task<IActionResult> Index()
        {
            var homeMetrics = new DashboardViewModel();

            homeMetrics.TotalTeamsCount = await _database.Teams.CountAsync();
            homeMetrics.TotalPlayersCount = await _database.Players.CountAsync();
            homeMetrics.TotalTournamentsCount = await _database.Tournaments.CountAsync();

            homeMetrics.UpcomingTournaments = await _database.Tournaments
                .OrderBy(tournament => tournament.EventDate)
                .Take(5)
                .ToListAsync();



            return View(homeMetrics);
        }
    }
}
