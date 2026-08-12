using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BasketbalFantasyApp.DAL;
using BasketbalFantasyApp.Models;

namespace BasketbalFantasyApp.Controllers
{
    [Authorize]
    public class TournamentGameplayController : Controller
    {
        private readonly BasketbalFantasyDbContext _database;

        public TournamentGameplayController(BasketbalFantasyDbContext database)
        {
            _database = database;
        }



        
    }
}
