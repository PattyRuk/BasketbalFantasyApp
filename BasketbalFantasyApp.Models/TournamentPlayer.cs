using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketbalFantasyApp.Models
{
    public class TournamentPlayer
    {
        public int TournamentId { get; set; }
        public Tournament? Tournament { get; set; }

        public int PlayerId { get; set; }
        public Player? Player { get; set; }

        public int PointsScored { get; set; }
        public int EfficiencyRating { get; set; }
    }
}
