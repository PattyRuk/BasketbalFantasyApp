using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BasketbalFantasyApp.DAL;
using BasketbalFantasyApp.Models;

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
    }
}