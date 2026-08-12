namespace BasketbalFantasyApp.Models
{
    public class TournamentGamesViewModel
    {
        public Tournament TournamentDetails { get; set; } = new Tournament();
        public List<Game> PlayedGames { get; set; } = new List<Game>();
        public List<PlayerStats> TournamentPlayerBoxScores { get; set; } = new List<PlayerStats>();
    }
}

