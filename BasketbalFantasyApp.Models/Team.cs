using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketbalFantasyApp.Models
{
    public class Team
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public string SponsorName { get; set; } = string.Empty;

        // One-to-One Link: Stores the unique Account ID of the user managing this team franchise
        public string OwnerUserId { get; set; } = string.Empty;

        // Navigation property: One team has a roster filled with multiple players
        public List<Player> Players { get; set; } = new List<Player>();
    }
}
