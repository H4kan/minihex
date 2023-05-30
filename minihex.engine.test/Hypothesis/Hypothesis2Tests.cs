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
        private const int NumberOfTestsForEachSeed = 5; // Game for each algorithm for each game size = NumberOfTestsForEachSeed*5*4
        private readonly List<int> GameSizes = new() { 5, 7, 9 };

        [DataTestMethod]
        [DataRow(false, "hypo2/winratio-results-noswap.txt")]
        [DataRow(true, "hypo2/winratio-results-swap.txt")]
        public void RunTests(bool swap, string fileName)
        {
            var resultList = new Dictionary<int, Dictionary<Algorithm, GameStats>>();
            foreach (var gameSize in GameSizes)
            {
                var resultsDictionary = TestHelpers.InitializeResultsDictionary();
                resultList.Add(gameSize, resultsDictionary);

                foreach (var (engineWhite, engineBlack) in TestHelpers.UniqueAlgorithmPairs())
                {
                    CalculateAlgorithmWins(engineWhite, engineBlack, gameSize, swap,
                        resultsDictionary[engineWhite], resultsDictionary[engineBlack]);
                    UpdateFileContent(resultList, fileName);
                }
            }
            UpdateFileContent(resultList, fileName);
        }

        private static void UpdateFileContent(Dictionary<int, Dictionary<Algorithm, GameStats>> results, string fileName)
        {
            var lines = new List<string>() { "Algorithm GameSize GamesWon GamesPlayed WinRatio MeanNumberOfWinningMoves" };
            foreach (var (gameSize, resultDictionary) in results)
            {
                foreach (var (engine, stats) in resultDictionary)
                {
                    lines.Add($"{engine} {gameSize} {stats.GamesWon} {stats.GamesPlayed} {stats.WinRatio} {stats.MeanNumberOfWinningMoves}");
                }
            }

            WriterHelper.SaveContentToFile(lines, fileName);
        }

        private void CalculateAlgorithmWins(Algorithm engineWhite, Algorithm engineBlack, int gameSize, bool swap,
            GameStats whiteEngineStats, GameStats blackEngineStats)
        {
            foreach (var seed in _seedIterator)
            {
                RandomSource.SetSeed(seed);
                for (int i = 0; i < NumberOfTestsForEachSeed; i++)
                {
                    var config = TestHelpers.CreateEnginesConfiguration(gameSize, engineWhite, engineBlack, swap);
                    var gameSimulator = new GameSimulator(config);
                    if (gameSimulator.RunSimulation() == PlayerColor.White)
                    {
                        whiteEngineStats.GamesWon += 1;
                        whiteEngineStats.MovesInWonGames += gameSimulator.Game.Moves.Count;
                    }
                    else
                    {
                        blackEngineStats.GamesWon += 1;
                        blackEngineStats.MovesInWonGames += gameSimulator.Game.Moves.Count;
                    }

                    blackEngineStats.GamesPlayed += 1;
                    whiteEngineStats.GamesPlayed += 1;
                }
            }
        }
    }
}
