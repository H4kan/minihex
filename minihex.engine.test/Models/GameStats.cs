namespace minihex.engine.test.Models
{
    public class GameStats
    {
        public int GamesWon { get; set; }
        public int GamesPlayed { get; set; }
        public int MovesInWonGames { get; set; }

        public double WinRatio => GamesWon / (double)GamesPlayed;
        public double MeanNumberOfWinningMoves => MovesInWonGames / (double)GamesWon;
    }
}
