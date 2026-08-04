using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketbalFantasyApp.Models
{
    public class Tournament
    {
        public int TournamentId { get; set; }
        public string TournamentName { get; set; } = string.Empty;
        public string FormatType { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }

        // Navigation property:  Many-to-Many connection
        public List<TournamentPlayer> TournamentPlayers { get; set; } = new List<TournamentPlayer>();
    }
}
