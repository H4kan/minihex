using minihex.engine.Models.Enums;
using minihex.engine.Randoms;
using minihex.engine.test.Helpers;

namespace minihex.engine.test.Hypothesis
{
    /// <summary>
    /// Algorytm heurystyczny nigdy nie przegra zaczynając w podstawowym wariancie gry. 
    /// </summary>
    [TestClass]
    public class Hypothesis3Tests
    {
        private readonly SeedHelperIterator _seedIterator = new();
        private const int NumberOfRuns = 20; // numberOfGames = NumberOfRuns*5
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
            double totalWinRatioFactor = 0;

            foreach (var enemyAlg in TestHelpers.GetAllEngines())
            {
                double winRatio = CalculateWinRatioForAlgorithm(enemyAlg);
                totalWinRatioFactor += winRatio;
                lines.Add($"{enemyAlg} {winRatio}");
            }
            lines.Add($"Aggregated {totalWinRatioFactor / TestHelpers.GetAllEngines().Count()}");

            return lines;
        }

        private double CalculateWinRatioForAlgorithm(Algorithm enemyAlg)
        {
            int numberOfWins = 0;

            for (int i = 0; i < NumberOfRuns; i++)
            {
                numberOfWins += RunSimulationsAndCountWins(enemyAlg);
            }

            return numberOfWins / (double)(NumberOfRuns * _seedIterator.Count);
        }

        private int RunSimulationsAndCountWins(Algorithm enemyAlg)
        {
            int wins = 0;

            foreach (var seed in _seedIterator)
            {
                RandomSource.SetSeed(seed);
                var config = TestHelpers.CreateEnginesConfiguration(GameSize, enemyAlg);
                var gameSimulator = new GameSimulator(config);
                wins += gameSimulator.RunSimulation() == Model.Enums.PlayerColor.White ? 1 : 0;
            }

            return wins;
        }
    }
}
