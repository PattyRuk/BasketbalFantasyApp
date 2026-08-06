using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketbalFantasyApp.Models
{
    public class Player
    {
        public int Id { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Position { get; set; }
        public string? NbaTeam { get; set; }

        // One-to-One - unique Account ID of the user managing this player's fantasy rights
        public string? OwnerUserId { get; set; }

        // Foreign Key linking this player to their current fantasy league team
        public int TeamId { get; set; }
        public Team Team { get; set; }

        // box score statistics logs
        public ICollection<PlayerStats> Stats { get; set; } = new List<PlayerStats>();

        public List<TournamentPlayer> TournamentPlayers { get; set; } = new List<TournamentPlayer>();
    }
}
