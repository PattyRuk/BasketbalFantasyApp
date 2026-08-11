namespace BasketbalFantasyApp.Models
{
    public class AvailablePlayerViewModel
    {
        // Holds athletes currently unassigned to a managed team
        public List<Player> AvailablePlayers { get; set; } = new List<Player>();
    }
}

