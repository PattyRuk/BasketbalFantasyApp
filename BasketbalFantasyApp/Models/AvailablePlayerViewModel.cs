namespace BasketbalFantasyApp.Models
{
    public class AvailablePlayerViewModel
    {
        // Holds athletes currently unassigned to any custom managed team
        public List<Player> AvailablePlayers { get; set; } = new List<Player>();
    }
}

