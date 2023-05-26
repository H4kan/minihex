using minihex.engine.Model.Enums;
using minihex.engine.Models.Enums;
using minihex.engine.Randoms;
using minihex.engine.test.Helpers;
using minihex.engine.test.Models;

namespace minihex.engine.test.Hypothesis
{
    /// <summary>
    /// Modyfikacja AMAF będzie miała większy wpływ na polepszenie jakości algorytmu MCTS niż savebridge pattern,
    /// jednak to zastosowanie obydwóch heurystyk da najlepsze wyniki
    /// </summary>
    [TestClass]
    public class Hypothesis2Tests
    {
        private readonly SeedHelperIterator _seedIterator = new();
        private const int NumberOfTestsForEachSeed = 2;
        private readonly List<int> GameSizes = new() { 5, 7, 9, 11 };

        [DataTestMethod]
        [DataRow(false, "hypo2/winratio-results-noswap.txt")]
        [DataRow(true, "hypo2/winratio-results-swap.txt")]
        public void RunTests(bool swap, string fileName)
        {
            var lines = new List<string>() { "Algorithm GameSize WinRatio MeanNumberOfWinningMoves" };
            foreach (var gameSize in GameSizes)
            {
                var resultsDictionary = InitializeResultsDictionary();
                foreach (var (engineWhite, engineBlack) in UniqueAlgorithmPairs())
                {
                    CalculateAlgorithmWins(engineWhite, engineBlack, gameSize, swap,
                        resultsDictionary[engineWhite], resultsDictionary[engineBlack]);
                }

                foreach (var (engine, stats) in resultsDictionary)
                {
                    lines.Add($"{engine} {gameSize} {stats.WinRatio} {stats.MeanNumberOfWinningMoves}");
                }
            }
            WriterHelper.SaveContentToFile(lines, fileName);
        }

        private static Dictionary<Algorithm, GameStats> InitializeResultsDictionary()
        {
            return Enum.GetValues(typeof(Algorithm))
                .Cast<Algorithm>()
                .ToDictionary(algo => algo, algo => new GameStats { GamesWon = 0, GamesPlayed = 0 });
        }

        private void CalculateAlgorithmWins(Algorithm engineWhite, Algorithm engineBlack, int gameSize, bool swap,
            GameStats whiteEngineStats, GameStats blackEngineStats)
        {
            for (int i = 0; i < NumberOfTestsForEachSeed; i++)
            {
                foreach (var seed in _seedIterator)
                {
                    RandomSource.SetSeed(seed);
                    var config = CreateEnginesConfiguration(gameSize, engineWhite, engineBlack, swap);
                    var gameSimulator = new GameSimulator(config);
                    if (gameSimulator.RunSimulation() == PlayerColor.White)
                    {
                        whiteEngineStats.GamesWon += 1;
                        whiteEngineStats.MovesInWonGames += gameSimulator.Game.Moves.Count();
                    }
                    else
                    {
                        blackEngineStats.GamesWon += 1;
                        blackEngineStats.MovesInWonGames += gameSimulator.Game.Moves.Count();
                    }

                    blackEngineStats.GamesPlayed += 1;
                    whiteEngineStats.GamesPlayed += 1;
                }
            }
        }

        public static EnginesSetupConfiguration CreateEnginesConfiguration(int gameSize, Algorithm engine1, Algorithm engine2, bool swap)
        {
            return new EnginesSetupConfiguration
            {
                Engine1 = engine1,
                Engine2 = engine2,
                GameSize = gameSize,
                GameSwapMode = swap,
            };
        }

        public static IEnumerable<(Algorithm, Algorithm)> UniqueAlgorithmPairs()
        {
            var values = Enum.GetValues(typeof(Algorithm)).Cast<Algorithm>().ToList().Skip(1);
            return values.SelectMany((value, index) => values.Skip(index + 1),
                                     (first, second) => (first, second));
        }
    }
}
