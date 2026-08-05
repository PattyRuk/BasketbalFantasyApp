using BasketbalFantasyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BasketbalFantasyApp.DAL
{
    public class BasketballApiService
    {
        private readonly HttpClient _httpClient;

        public BasketballApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "BasketballFantasyLeagueApp");
        }

        public async Task<List<Player>> FetchAndParseNbaPlayersAsync(string apiKey, int fallbackTeamId)
        {
            var resultsList = new List<Player>();

            // BallDon'tLie API URL
            var request = new HttpRequestMessage(HttpMethod.Get, "https://balldontlie.io");
            request.Headers.Authorization = new AuthenticationHeaderValue(apiKey);

            try
            {
                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    using var document = JsonDocument.Parse(jsonString);
                    var dataRoot = document.RootElement.GetProperty("data");

                    foreach (var element in dataRoot.EnumerateArray())
                    {
                        var player = new Player
                        {
                            Id = element.GetProperty("id").GetInt32(),
                            FirstName = element.GetProperty("first_name").GetString() ?? "",
                            LastName = element.GetProperty("last_name").GetString() ?? "",
                            Position = element.GetProperty("position").GetString() ?? "Guard",
                            NbaTeam = element.GetProperty("team").GetProperty("full_name").GetString() ?? "Free Agent",
                            TeamId = fallbackTeamId, // Links database records to entity tables
                            OwnerUserId = "SYSTEM_POOL"
                        };
                        resultsList.Add(player);
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Network failure parsing live basketball API metrics: {exception.Message}");
            }

            return resultsList;
        }
    }
}
