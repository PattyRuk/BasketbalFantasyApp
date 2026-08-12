namespace BasketbalFantasyApp.Models
{
    public class TournamentSummaryViewModel
    {
        public Tournament TournamentDetails { get; set; } = new Tournament();
        public List<TournamentTeam> Standings { get; set; } = new List<TournamentTeam>();

        // Winner specific highlights
        public Team WinnerTeam { get; set; } = new Team();
        public List<Player> WinnerRoster { get; set; } = new List<Player>();

        // MVP parameters
        public Player TournamentMvp { get; set; } = new Player();
        public double MvpAveragePoints { get; set; }
        public int MvpTotalPoints { get; set; }
    }
}