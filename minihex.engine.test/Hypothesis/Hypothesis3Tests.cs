using minihex.engine.Models.Enums;
using minihex.engine.Randoms;
using minihex.engine.test.Helpers;
using minihex.engine.test.Models;

namespace minihex.engine.test.Hypothesis
{
    /// <summary>
    /// Algorytm heurystyczny nigdy nie przegra zaczynając w podstawowym wariancie gry. 
    /// </summary>
    [TestClass]
    public class Hypothesis3Tests
    {
        private readonly SeedHelperIterator _seedIterator = new();
        private const int NumberOfTests = 2;
        private const int GameSize = 7;
        
        [TestMethod]
        public void RunTests()
        {
            var results = CalculateWinRatiosForAlgorithms();
            WriterHelper.SaveContentToFile(results, "hypo3/winratio-results.txt");
        }

        private List<string> CalculateWinRatiosForAlgorithms()
        {
            var lines = new List<string>() { "Algorithm WinRatio" };

            foreach (var enemyAlg in EnemyEngines())
            {
                double winRatio = CalculateWinRatioForAlgorithm(enemyAlg);
                lines.Add($"{enemyAlg} {winRatio}");
            }

            return lines;
        }

        private double CalculateWinRatioForAlgorithm(Algorithm enemyAlg)
        {
            int numberOfWins = 0;

            for (int i = 0; i < NumberOfTests; i++)
            {
                numberOfWins += RunSimulationsAndCountWins(enemyAlg);
            }

            return numberOfWins / (double)(NumberOfTests * _seedIterator.Count);
        }

        private int RunSimulationsAndCountWins(Algorithm enemyAlg)
        {
            int wins = 0;

            foreach (var seed in _seedIterator)
            {
                RandomSource.SetSeed(seed);
                var config = CreateEnginesConfiguration(GameSize, enemyAlg);
                var gameSimulator = new GameSimulator(config);
                wins += gameSimulator.RunSimulation() == Model.Enums.PlayerColor.White ? 1 : 0;
            }

            return wins;
        }

        public static EnginesSetupConfiguration CreateEnginesConfiguration(int gameSize, Algorithm blackEngine)
        {
            return new EnginesSetupConfiguration
            {
                Engine1 = Algorithm.Heuristic,
                Engine2 = blackEngine,
                GameSize = gameSize,
                GameSwapMode = false,
            };
        }

        public static IEnumerable<Algorithm> EnemyEngines()
        {
            yield return Algorithm.Heuristic;
            yield return Algorithm.MCTS;
            yield return Algorithm.MCTSwSavebridge;
            yield return Algorithm.MSTSwAMAF;
            yield return Algorithm.MCTSwAMAFandSavebridge;
        }
    }
}
