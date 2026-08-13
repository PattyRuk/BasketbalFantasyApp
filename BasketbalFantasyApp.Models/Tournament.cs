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
        public string? TournamentName { get; set; } 
        public string? FormatType { get; set; }
        public DateTime EventDate { get; set; }

        // Games required to win the tournament
        public int RequiredWins { get; set; } = 3;
        public bool IsCompleted { get; set; } = false;

        // Championship Logs
        public int? WinnerTeamId { get; set; }
        public Team? WinnerTeam { get; set; }
        public int? MvpPlayerId { get; set; }
        public Player? MvpPlayer { get; set; }

        // Navigation:  Many-to-Many connection
        public List<TournamentPlayer> TournamentPlayers { get; set; } = new List<TournamentPlayer>();
        public List<TournamentTeam> TournamentTeams { get; set; } = new List<TournamentTeam>();
        public List<Game> Games { get; set; } = new List<Game>();

    }
}
