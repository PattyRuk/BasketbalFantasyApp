namespace BasketbalFantasyApp.Models
{
    public class PlayerSummaryViewModel
    {
        public Player PlayerBio { get; set; } = new Player();
        public Team DraftedTeam { get; set; } = new Team();

        // Analytical Targets
        public int TotalGamesPlayed { get; set; }
        public int TotalPointsScored { get; set; }
        public double AveragePointsPerGame { get; set; }
        public double AverageRebounds { get; set; }
        public double AverageAssists { get; set; }

        // Tournament History Breakdown
        public List<TournamentStatsRow> PerformanceHistory { get; set; } = new List<TournamentStatsRow>();
    }

    public class TournamentStatsRow
    {
        public string? TournamentName { get; set; }
        public int GamesPlayed { get; set; }
        public int PointsScored { get; set; }
        public double FieldGoalPercentage { get; set; }
    }
}
