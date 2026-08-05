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

            }
            catch (Exception exception)
            {
                Console.WriteLine($"Network failure parsing live basketball API metrics: {exception.Message}");
            }

            return resultsList;
        }
    }
}
