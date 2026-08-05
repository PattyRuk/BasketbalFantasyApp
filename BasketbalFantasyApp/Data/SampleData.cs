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

            // 2. Fetches data from the API if tables are empty
            if (!database.Players.Any())
            {
                var placementTeam = database.Teams.First();
                var apiService = new BasketballApiService();

                // active API token credential 
                string activeApiKey = "15c951ab-6eae-4f2f-b3d5-962cb5da3190";
                var realPlayers = await apiService.FetchAndParseNbaPlayersAsync(activeApiKey, placementTeam.TeamId);

                if (realPlayers.Any())
                {
                    database.Players.AddRange(realPlayers);
                    await database.SaveChangesAsync();

                    // Seed random mock statistics data lines for the downloaded players
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

            // 3.Sample upcoming tournament matchups if empty
            if (!database.Tournaments.Any())
            {
                database.Tournaments.AddRange(new List<Tournament>
                {
                    new Tournament { TournamentName = "Summer Elite Cup", FormatType = "3x3 Half-Court", EventDate = DateTime.Now.AddDays(14) },
                    new Tournament { TournamentName = "Pro-Am Championship", FormatType = "Full Court Elimination", EventDate = DateTime.Now.AddDays(30) }
                });
                await database.SaveChangesAsync();
            }
        }
    }
}
