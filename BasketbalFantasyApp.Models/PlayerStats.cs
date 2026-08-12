using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketbalFantasyApp.Models
{
    public class PlayerStats
    {
        public int Id { get; set; }

        // Foreign Key pointing back to the specific player profile
        public int PlayerId { get; set; }
        public int? TournamentId { get; set; }
        public DateTime GameDate { get; set; }

        // Traditional Basketball Statistical Categories
        public int Points { get; set; }
        public int Rebounds { get; set; }
        public int Assists { get; set; }
        public int Steals { get; set; }
        public int Blocks { get; set; }
        public int Turnovers { get; set; }
        public int ThreePointersMade { get; set; }
        public double FieldGoalPercentage { get; set; }
        public double FreeThrowPercentage { get; set; }

        // Navigation property
        public Player? Player { get; set; }
        public Tournament? Tournament { get; set; }
    }
}
