namespace BasketbalFantasyApp.Models
{
    public class TournamentRegisterViewModel
    {
        public int TournamentId { get; set; }
        public string TournamentName { get; set; } = string.Empty;
        public string FormatType { get; set; } = string.Empty;
        public int RequiredPlayersCount { get; set; } // 3 or 5 based on layout constraints

        public List<Player> EligibleRoster { get; set; } = new List<Player>();
    }
}
