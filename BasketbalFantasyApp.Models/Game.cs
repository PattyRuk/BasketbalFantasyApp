using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketbalFantasyApp.Models
{
    public class Game
    {
        public int GameId { get; set; }
        public int TournamentId { get; set; }
        public int TeamAId { get; set; }
        public int TeamBId { get; set; }
        public int TeamAScore { get; set; }
        public int TeamBScore { get; set; }
        public DateTime MatchTimestamp { get; set; }

        public int? WinnerTeamId { get; set; }

        // Navigation
        public Tournament? Tournament { get; set; }
        public Team? TeamA { get; set; }
        public Team? TeamB { get; set; }

    }
}