namespace BasketbalFantasyApp.Models
{
    public class DashboardViewModel
    {
        public int TotalTeamsCount { get; set; }
        public int TotalPlayersCount { get; set; }
        public int TotalTournamentsCount { get; set; }
        public bool HasTeam { get; set; }
        public string? UserTeamName { get; set; }
        public string? UserTeamSponsor { get; set; }
        public List<Player> MyRosterPlayers { get; set; } = new List<Player>();

        public List<Tournament> UpcomingTournaments { get; set; } = new List<Tournament>();
    }
}
