using BasketbalFantasyApp.DAL;
using BasketbalFantasyApp.Models;

namespace BasketbalFantasyApp.Data
{
    public static class SampleData
    {
        public static async Task SeedDatabaseAsync(BasketbalFantasyDbContext database)
        {
            // 1. Default initial pool team 
            if (!database.Teams.Any())
            {
                var baseTeam = new Team
                {
                    TeamName = "League Free Agent Pool",
                    SponsorName = "Global Sports Media",
                    OwnerUserId = "SYSTEM_INITIAL_ACCOUNT"
                };
                database.Teams.Add(baseTeam);
                await database.SaveChangesAsync();
            }


        }
    }
}
