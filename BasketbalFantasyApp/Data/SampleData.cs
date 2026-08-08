using BasketbalFantasyApp.DAL;
using BasketbalFantasyApp.Models;

namespace BasketbalFantasyApp.Data
{
    public static class SampleData
    {
        public static async Task SeedDatabaseAsync(BasketbalFantasyDbContext database)
        {
            // 1. Default initial pool team 
            if (!await database.Teams.AnyAsync())
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

            // 2. Fetches data from the API if tables are empty 
            if (!await database.Players.AnyAsync())
            {
                var placementTeam = await database.Teams.FirstOrDefaultAsync();

                if (placementTeam != null)
                {
                    var apiService = new BasketballApiService();

                    // Active BallDon'tLie token key
                    string activeApiKey = "15c951ab-6eae-4f2f-b3d5-962cb5da3190";
                    var realPlayers = await apiService.FetchAndParseNbaPlayersAsync(activeApiKey, placementTeam.TeamId);

                    if (realPlayers != null && realPlayers.Any())
                    {
                        database.Players.AddRange(realPlayers);
                        await database.SaveChangesAsync();

                        //sample statistic for newly downloaded players
                        foreach (var player in database.Players.Take(5))
                        {
                            database.PlayerStats.Add(new PlayerStats
                            {
                                PlayerId = player.Id,
                                GameDate = DateTime.Now.AddDays(-1),
                                Points = 24,
                                Rebounds = 8,
                                Assists = 6,
                                Steals = 2,
                                Blocks = 1,
                                Turnovers = 3,
                                ThreePointersMade = 4,
                                FieldGoalPercentage = 0.485,
                                FreeThrowPercentage = 0.820
                            });
                        }
                        await database.SaveChangesAsync();
                    }
                }
            }

            // 3. Sample upcoming tournament events if empty
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
