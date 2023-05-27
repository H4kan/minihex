using minihex.engine.Model.Enums;
using minihex.engine.Models.Enums;
using minihex.engine.Randoms;
using minihex.engine.test.Helpers;

namespace minihex.engine.test.Hypothesis
{
    /// <summary>
    /// Od pewnej liczby iteracji ekspansji MCTS, algorytmy na nim oparte 
    /// prawie nigdy nie będą przegrywać zaczynając w podstawowym wariancie gry
    /// </summary>
    [TestClass]
    public class Hypothesis4Tests
    {
        private readonly SeedHelperIterator _seedIterator = new();
        private const int NumberOfTestsForEachSeedAndEngine = 2; // numberOfGamesForEachIteration = NumberOfTestsForEachSeedAndEngine*25
        private readonly List<int> GameSizes = new() { 5, 9 };
        private const int IterationStep = 50;
        private readonly IEnumerable<int> IterationsRange = Enumerable.Range(1, 10).Select(i => i * IterationStep);


        [TestMethod]
        public void RunTests()
        {
            foreach (var gameSize in GameSizes)
            {
                var lines = new List<string>() { "Algorithm Iterations WinRatio" };
                foreach (var engine in TestHelpers.GetMCTSEngines())
                {
                    foreach (var iter in IterationsRange)
                    {
                        double winRatio = CalculateWinRatioForAlgorithm(engine, iter, gameSize);
                        lines.Add($"{engine} {iter} {winRatio}");

                        WriterHelper.SaveContentToFile(lines, $"hypo4/winratio-mcts-start-noswap-iterations-results-hex{gameSize}.txt");
                    }
                }

                WriterHelper.SaveContentToFile(lines, $"hypo4/winratio-mcts-start-noswap-iterations-results-hex{gameSize}.txt");
            }
        }

        private double CalculateWinRatioForAlgorithm(Algorithm engine, int iterations, int gameSize)
        {
            int numberOfWins = 0;
            var enemiesEngines = TestHelpers.GetAllEngines();

            for (int i = 0; i < NumberOfTestsForEachSeedAndEngine; i++)
            {
                foreach (var enemy in enemiesEngines)
                {
                    numberOfWins += RunSimulationsAndCountWins(engine, enemy, iterations, gameSize, PlayerColor.White);
                }
            }

            return numberOfWins / (double)(NumberOfTestsForEachSeedAndEngine * _seedIterator.Count * enemiesEngines.Count());
        }

        private int RunSimulationsAndCountWins(Algorithm whiteAlg, Algorithm blackAlg, int iterations, int gameSize, PlayerColor expectedToWin)
        {
            int wins = 0;
            foreach (var seed in _seedIterator)
            {
                RandomSource.SetSeed(seed);
                var config = TestHelpers.CreateEnginesConfiguration(gameSize, whiteAlg, blackAlg, iterations, false);
                var gameSimulator = new GameSimulator(config);
                wins += gameSimulator.RunSimulation() == expectedToWin ? 1 : 0;
            }

            return wins;
        }
    }
}
