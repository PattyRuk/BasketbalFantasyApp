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
            // 1. Default System Team to hold unassigned players pool
            if (!await database.Teams.AnyAsync())
            {
                using (var transaction = await database.Database.BeginTransactionAsync())
                {
                    try
                    {

                        // Temporarily allow explicit key inserts for the Team table(this was needed to allow formation of new teams without confilcts in team id)
                        await database.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Teams ON");
                        var systemPoolTeam = new Team
                        {
                            TeamId = 1, // Fixes the ID for lookups
                            TeamName = "Global Player Pool",
                            SponsorName = "League Collective",
                            OwnerUserId = "SYSTEM_POOL"
                        };

                        database.Teams.Add(systemPoolTeam);
                        await database.SaveChangesAsync();
                        await database.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Teams OFF");
                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw; // Let the developer see if a secondary constraint failed
                    }
                }
            }

            // 2. Injection for the Basketball Players
            if (!await database.Players.AnyAsync())
            {
                using (var transaction = await database.Database.BeginTransactionAsync())
                {
                    try
                    {
                        await database.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Players ON");
                        var playersPoolList = new List<Player>
                        {

                            // Boston Celtics
                            new Player { Id = 101, FirstName = "Jayson", LastName = "Tatum", Position = "Forward", NbaTeam = "Boston Celtics", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 102, FirstName = "Jaylen", LastName = "Brown", Position = "Forward", NbaTeam = "Boston Celtics", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 103, FirstName = "Jrue", LastName = "Holiday", Position = "Guard", NbaTeam = "Boston Celtics", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },

                            // New York Knicks
                            new Player { Id = 104, FirstName = "Jalen", LastName = "Brunson", Position = "Guard", NbaTeam = "New York Knicks", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 105, FirstName = "Karl-Anthony", LastName = "Towns", Position = "Center", NbaTeam = "New York Knicks", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 106, FirstName = "OG", LastName = "Anunoby", Position = "Forward", NbaTeam = "New York Knicks", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },

                            // Philadelphia 76ers
                            new Player { Id = 107, FirstName = "Joel", LastName = "Embiid", Position = "Center", NbaTeam = "Philadelphia 76ers", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 108, FirstName = "Tyrese", LastName = "Maxey", Position = "Guard", NbaTeam = "Philadelphia 76ers", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 109, FirstName = "Paul", LastName = "George", Position = "Forward", NbaTeam = "Philadelphia 76ers", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },

                            // Cleveland Cavaliers
                            new Player { Id = 110, FirstName = "Donovan", LastName = "Mitchell", Position = "Guard", NbaTeam = "Cleveland Cavaliers", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 111, FirstName = "Darius", LastName = "Garland", Position = "Guard", NbaTeam = "Cleveland Cavaliers", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 112, FirstName = "Evan", LastName = "Mobley", Position = "Forward", NbaTeam = "Cleveland Cavaliers", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },

                            // Milwaukee Bucks
                            new Player { Id = 113, FirstName = "Giannis", LastName = "Antetokounmpo", Position = "Forward", NbaTeam = "Milwaukee Bucks", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 114, FirstName = "Damian", LastName = "Lillard", Position = "Guard", NbaTeam = "Milwaukee Bucks", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 115, FirstName = "Khris", LastName = "Middleton", Position = "Forward", NbaTeam = "Milwaukee Bucks", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },

                            // Indiana Pacers
                            new Player { Id = 116, FirstName = "Tyrese", LastName = "Haliburton", Position = "Guard", NbaTeam = "Indiana Pacers", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 117, FirstName = "Pascal", LastName = "Siakam", Position = "Forward", NbaTeam = "Indiana Pacers", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 118, FirstName = "Myles", LastName = "Turner", Position = "Center", NbaTeam = "Indiana Pacers", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },

                            // Orlando Magic
                            new Player { Id = 119, FirstName = "Paolo", LastName = "Banchero", Position = "Forward", NbaTeam = "Orlando Magic", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 120, FirstName = "Franz", LastName = "Wagner", Position = "Forward", NbaTeam = "Orlando Magic", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 121, FirstName = "Jalen", LastName = "Suggs", Position = "Guard", NbaTeam = "Orlando Magic", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },

                            // Miami Heat
                            new Player { Id = 122, FirstName = "Jimmy", LastName = "Butler", Position = "Forward", NbaTeam = "Miami Heat", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 123, FirstName = "Bam", LastName = "Adebayo", Position = "Center", NbaTeam = "Miami Heat", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 124, FirstName = "Tyler", LastName = "Herro", Position = "Guard", NbaTeam = "Miami Heat", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },

                            // Chicago Bulls
                            new Player { Id = 125, FirstName = "Zach", LastName = "LaVine", Position = "Forward", NbaTeam = "Chicago Bulls", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 126, FirstName = "Coby", LastName = "White", Position = "Guard", NbaTeam = "Chicago Bulls", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 127, FirstName = "Josh", LastName = "Giddey", Position = "Guard", NbaTeam = "Chicago Bulls", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },

                            // Atlanta Hawks
                            new Player { Id = 128, FirstName = "Trae", LastName = "Young", Position = "Guard", NbaTeam = "Atlanta Hawks", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 129, FirstName = "Jalen", LastName = "Johnson", Position = "Forward", NbaTeam = "Atlanta Hawks", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 130, FirstName = "Dyson", LastName = "Daniels", Position = "Guard", NbaTeam = "Atlanta Hawks", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },

                            // Brooklyn Nets
                            new Player { Id = 131, FirstName = "Cam", LastName = "Thomas", Position = "Guard", NbaTeam = "Brooklyn Nets", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 132, FirstName = "Cameron", LastName = "Johnson", Position = "Forward", NbaTeam = "Brooklyn Nets", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 133, FirstName = "Nic", LastName = "Claxton", Position = "Center", NbaTeam = "Brooklyn Nets", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },

                            // Toronto Raptors
                            new Player { Id = 134, FirstName = "Scottie", LastName = "Barnes", Position = "Forward", NbaTeam = "Toronto Raptors", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 135, FirstName = "RJ", LastName = "Barrett", Position = "Forward", NbaTeam = "Toronto Raptors", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 136, FirstName = "Immanuel", LastName = "Quickley", Position = "Guard", NbaTeam = "Toronto Raptors", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },

                            // Charlotte Hornets
                            new Player { Id = 137, FirstName = "LaMelo", LastName = "Ball", Position = "Guard", NbaTeam = "Charlotte Hornets", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 138, FirstName = "Brandon", LastName = "Miller", Position = "Guard", NbaTeam = "Charlotte Hornets", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 139, FirstName = "Miles", LastName = "Bridges", Position = "Forward", NbaTeam = "Charlotte Hornets", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },

                            // Washington Wizards
                            new Player { Id = 140, FirstName = "Jordan", LastName = "Poole", Position = "Guard", NbaTeam = "Washington Wizards", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 141, FirstName = "Kyle", LastName = "Kuzma", Position = "Forward", NbaTeam = "Washington Wizards", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 142, FirstName = "Alex", LastName = "Sarr", Position = "Forward", NbaTeam = "Washington Wizards", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },

                            // Detroit Pistons
                            new Player { Id = 143, FirstName = "Cade", LastName = "Cunningham", Position = "Guard", NbaTeam = "Detroit Pistons", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 144, FirstName = "Jaden", LastName = "Ivey", Position = "Guard", NbaTeam = "Detroit Pistons", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 145, FirstName = "Tobias", LastName = "Harris", Position = "Forward", NbaTeam = "Detroit Pistons", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },

                            // Oklahoma City Thunder
                            new Player { Id = 146, FirstName = "Shai", LastName = "Gilgeous-Alexander", Position = "Guard", NbaTeam = "Oklahoma City Thunder", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 147, FirstName = "Jalen", LastName = "Williams", Position = "Forward", NbaTeam = "Oklahoma City Thunder", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 148, FirstName = "Chet", LastName = "Holmgren", Position = "Center", NbaTeam = "Oklahoma City Thunder", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },

                            // Denver Nuggets
                            new Player { Id = 149, FirstName = "Nikola", LastName = "Jokic", Position = "Center", NbaTeam = "Denver Nuggets", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 150, FirstName = "Jamal", LastName = "Murray", Position = "Guard", NbaTeam = "Denver Nuggets", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 151, FirstName = "Michael", LastName = "Porter", Position = "Forward", NbaTeam = "Denver Nuggets", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },

                            // Minnesota Timberwolves
                            new Player { Id = 152, FirstName = "Anthony", LastName = "Edwards", Position = "Guard", NbaTeam = "Minnesota Timberwolves", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 153, FirstName = "Julius", LastName = "Randle", Position = "Forward", NbaTeam = "Minnesota Timberwolves", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 154, FirstName = "Rudy", LastName = "Gobert", Position = "Center", NbaTeam = "Minnesota Timberwolves", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },

                            // LA Clippers
                            new Player { Id = 155, FirstName = "James", LastName = "Harden", Position = "Guard", NbaTeam = "LA Clippers", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 156, FirstName = "Kawhi", LastName = "Leonard", Position = "Forward", NbaTeam = "LA Clippers", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 157, FirstName = "Norman", LastName = "Powell", Position = "Forward", NbaTeam = "LA Clippers", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },

                            // Dallas Mavericks
                            new Player { Id = 158, FirstName = "Luka", LastName = "Doncic", Position = "Guard", NbaTeam = "Dallas Mavericks", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 159, FirstName = "Kyrie", LastName = "Irving", Position = "Guard", NbaTeam = "Dallas Mavericks", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 160, FirstName = "Klay", LastName = "Thompson", Position = "Forward", NbaTeam = "Dallas Mavericks", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },

                            // Phoenix Suns
                            new Player { Id = 161, FirstName = "Kevin", LastName = "Durant", Position = "Forward", NbaTeam = "Phoenix Suns", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 162, FirstName = "Devin", LastName = "Booker", Position = "Guard", NbaTeam = "Phoenix Suns", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 163, FirstName = "Bradley", LastName = "Beal", Position = "Forward", NbaTeam = "Phoenix Suns", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },

                            // New Orleans Pelicans
                            new Player { Id = 164, FirstName = "Zion", LastName = "Williamson", Position = "Forward", NbaTeam = "New Orleans Pelicans", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 165, FirstName = "Brandon", LastName = "Ingram", Position = "Forward", NbaTeam = "New Orleans Pelicans", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 166, FirstName = "Dejounte", LastName = "Murray", Position = "Guard", NbaTeam = "New Orleans Pelicans", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },

                            // Los Angeles Lakers
                            new Player { Id = 167, FirstName = "LeBron", LastName = "James", Position = "Forward", NbaTeam = "Los Angeles Lakers", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 168, FirstName = "Anthony", LastName = "Davis", Position = "Center", NbaTeam = "Los Angeles Lakers", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 169, FirstName = "Austin", LastName = "Reaves", Position = "Guard", NbaTeam = "Los Angeles Lakers", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },

                            // Sacramento Kings
                            new Player { Id = 170, FirstName = "De'Aaron", LastName = "Fox", Position = "Guard", NbaTeam = "Sacramento Kings", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 171, FirstName = "Domantas", LastName = "Sabonis", Position = "Center", NbaTeam = "Sacramento Kings", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 172, FirstName = "DeMar", LastName = "DeRozan", Position = "Forward", NbaTeam = "Sacramento Kings", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },

                            // Golden State Warriors
                            new Player { Id = 173, FirstName = "Stephen", LastName = "Curry", Position = "Guard", NbaTeam = "Golden State Warriors", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 174, FirstName = "Draymond", LastName = "Green", Position = "Center", NbaTeam = "Golden State Warriors", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 175, FirstName = "Andrew", LastName = "Wiggins", Position = "Forward", NbaTeam = "Golden State Warriors", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },

                            // Houston Rockets
                            new Player { Id = 176, FirstName = "Alperen", LastName = "Sengun", Position = "Center", NbaTeam = "Houston Rockets", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 177, FirstName = "Jalen", LastName = "Green", Position = "Guard", NbaTeam = "Houston Rockets", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 178, FirstName = "Fred", LastName = "VanVleet", Position = "Guard", NbaTeam = "Houston Rockets", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },

                            // Utah Jazz
                            new Player { Id = 179, FirstName = "Lauri", LastName = "Markkanen", Position = "Forward", NbaTeam = "Utah Jazz", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 180, FirstName = "Collin", LastName = "Sexton", Position = "Guard", NbaTeam = "Utah Jazz", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 181, FirstName = "Keyonte", LastName = "George", Position = "Guard", NbaTeam = "Utah Jazz", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },

                            // Memphis Grizzlies
                            new Player { Id = 182, FirstName = "Ja", LastName = "Morant", Position = "Guard", NbaTeam = "Memphis Grizzlies", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 183, FirstName = "Jaren", LastName = "Jackson", Position = "Forward", NbaTeam = "Memphis Grizzlies", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 184, FirstName = "Desmond", LastName = "Bane", Position = "Guard", NbaTeam = "Memphis Grizzlies", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },

                            // San Antonio Spurs
                            new Player { Id = 185, FirstName = "Victor", LastName = "Wembanyama", Position = "Center", NbaTeam = "San Antonio Spurs", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 186, FirstName = "Chris", LastName = "Paul", Position = "Guard", NbaTeam = "San Antonio Spurs", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 187, FirstName = "Devin", LastName = "Vassell", Position = "Guard", NbaTeam = "San Antonio Spurs", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },

                            // Portland Trail Blazers
                            new Player { Id = 188, FirstName = "Jerami", LastName = "Grant", Position = "Forward", NbaTeam = "Portland Trail Blazers", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 189, FirstName = "Anfernee", LastName = "Simons", Position = "Guard", NbaTeam = "Portland Trail Blazers", TeamId = 1, OwnerUserId = "SYSTEM_POOL" },
                            new Player { Id = 190, FirstName = "Deandre", LastName = "Ayton", Position = "Center", NbaTeam = "Portland Trail Blazers", TeamId = 1, OwnerUserId = "SYSTEM_POOL" }

                        };

                        database.Players.AddRange(playersPoolList);
                        await database.SaveChangesAsync();
                        await database.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Players OFF");
                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }

        }
    }
}

