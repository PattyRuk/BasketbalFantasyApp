using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketbalFantasyApp.Models
{
    public class TournamentTeam
    {
        public int TournamentId { get; set; }
        public int TeamId { get; set; }
        public int WinsCount { get; set; } = 0;
        public int LossesCount { get; set; } = 0;
        public string FinalPosition { get; set; } = "Contender"; // "Champion", "Runner-Up", "Eliminated"

        // tournament-specific rosters/players
        public List<Player> RegisteredPlayers { get; set; } = new List<Player>();

        // Navigation
        public Tournament? Tournament { get; set; }
        public Team? Team { get; set; }


    }
}