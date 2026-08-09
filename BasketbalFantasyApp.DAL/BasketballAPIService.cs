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

            // Updated base endpoint for the standard v1 API path
            var request = new HttpRequestMessage(HttpMethod.Get, "https://balldontlie.io");

            // This injects the key into the secure post-login handshake header layer
            request.Headers.Add("Authorization", apiKey);

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
                        string firstName = element.GetProperty("first_name").GetString() ?? "";
                        string lastName = element.GetProperty("last_name").GetString() ?? "";
                        string apiPosition = element.GetProperty("position").GetString() ?? "Guard";
                        string finalPosition = string.IsNullOrWhiteSpace(apiPosition) ? "G-F" : apiPosition;

                        resultsList.Add(new Player
                        {
                            Id = element.GetProperty("id").GetInt32(),
                            FirstName = firstName,
                            LastName = lastName,
                            Position = finalPosition,
                            // Navigates the API JSON object to grab the nested NBA club name
                            NbaTeam = element.GetProperty("team").GetProperty("full_name").GetString() ?? "Free Agent",
                            TeamId = fallbackTeamId,
                            OwnerUserId = "SYSTEM_INITIAL_POOL"
                        });
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"API Download Interrupted: {exception.Message}");
            }

            return resultsList;
        }
    }
}
