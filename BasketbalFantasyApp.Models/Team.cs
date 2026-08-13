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
        public string? TeamName { get; set; } 
        public string? SponsorName { get; set; } 

        // One-to-One - unique Account ID of the user managing the team 
        public string? OwnerUserId { get; set; }

        // Navigation - One team has a roster filled with multiple players
        public List<Player> Players { get; set; } = new List<Player>();
    }
}
