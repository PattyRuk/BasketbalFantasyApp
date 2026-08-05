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

        
    }
}
