using Microsoft.EntityFrameworkCore;
using BasketbalFantasyApp.DAL;
using BasketbalFantasyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketbalFantasyApp.Data
{
    public static class SampleData
    {
        public static async Task SeedDatabaseAsync(BasketbalFantasyDbContext database)
        {
            // 1. Seed the default System Team to hold the global unassigned players pool
            if (!await database.Teams.AnyAsync())
            {
                var systemPoolTeam = new Team
                {
                    TeamId = 1, // Fixes the ID for lookups
                    TeamName = "Global Player Pool",
                    SponsorName = "League Collective",
                    OwnerUserId = "SYSTEM_POOL"
                };

                database.Teams.Add(systemPoolTeam);
                await database.SaveChangesAsync();
            }

            // 2. Code-First Data Injection for the Basketball Players
            if (!await database.Players.AnyAsync())
            {
                var playersPoolList = new List<Player>
                {
                    new Player { Id = 101, FirstName = "LeBron", LastName = "James", Position = "Forward", NbaTeam = "Los Angeles Lakers", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                    new Player { Id = 102, FirstName = "Stephen", LastName = "Curry", Position = "Guard", NbaTeam = "Golden State Warriors", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                    new Player { Id = 103, FirstName = "Kevin", LastName = "Durant", Position = "Forward", NbaTeam = "Phoenix Suns", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                    new Player { Id = 104, FirstName = "Nikola", LastName = "Jokic", Position = "Center", NbaTeam = "Denver Nuggets", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                    new Player { Id = 105, FirstName = "Giannis", LastName = "Antetokounmpo", Position = "Forward", NbaTeam = "Milwaukee Bucks", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                    new Player { Id = 106, FirstName = "Luka", LastName = "Doncic", Position = "Guard", NbaTeam = "Dallas Mavericks", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                    new Player { Id = 107, FirstName = "Jayson", LastName = "Tatum", Position = "Forward", NbaTeam = "Boston Celtics", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                    new Player { Id = 108, FirstName = "Anthony", LastName = "Edwards", Position = "Guard", NbaTeam = "Minnesota Timberwolves", TeamId = 1, OwnerUserId = "SYSTEM_POOL" }
                };

                database.Players.AddRange(playersPoolList);
                await database.SaveChangesAsync();

                // 3. Code-First Data Injection for Player Statistics
                var sampleStatsList = new List<PlayerStats>
                {
                    new PlayerStats { PlayerId = 101, GameDate = DateTime.Now.AddDays(-1), Points = 26, Rebounds = 7, Assists = 8, Steals = 1, Blocks = 1, Turnovers = 3, ThreePointersMade = 2, FieldGoalPercentage = 0.520, FreeThrowPercentage = 0.780 },
                    new PlayerStats { PlayerId = 102, GameDate = DateTime.Now.AddDays(-1), Points = 31, Rebounds = 4, Assists = 6, Steals = 2, Blocks = 0, Turnovers = 2, ThreePointersMade = 6, FieldGoalPercentage = 0.465, FreeThrowPercentage = 0.910 },
                    new PlayerStats { PlayerId = 104, GameDate = DateTime.Now.AddDays(-1), Points = 28, Rebounds = 12, Assists = 10, Steals = 1, Blocks = 1, Turnovers = 4, ThreePointersMade = 1, FieldGoalPercentage = 0.580, FreeThrowPercentage = 0.820 }
                };

                database.PlayerStats.AddRange(sampleStatsList);
                await database.SaveChangesAsync();
            }

            // 4. Seed tournament events matching layout requirements
            if (!await database.Tournaments.AnyAsync())
            {
                database.Tournaments.AddRange(new List<Tournament>
                {
                    new Tournament { TournamentName = "Summer Elite Cup", FormatType = "3x3 Half-Court", EventDate = DateTime.Now.AddDays(14) },
                    new Tournament { TournamentName = "Pro-Am Championship", FormatType = "5x5 Full Court", EventDate = DateTime.Now.AddDays(30) }
                });
                await database.SaveChangesAsync();
            }
        }
    }
}

