using minihex.engine.Model.Enums;
using minihex.engine.Models.Enums;
using minihex.engine.Randoms;
using minihex.engine.test.Helpers;

namespace minihex.engine.test.Hypothesis
{
    /// <summary>
    /// Od pewnej liczby iteracji ekspansji MCTS, algorytmy na nim oparte zazwyczaj
    /// będą wygrywać z algorytmem heurystycznym w wariancie SWAP
    /// </summary>
    [TestClass]
    public class Hypothesis56Tests
    {
        private readonly SeedHelperIterator _seedIterator = new();
        private const int NumberOfTestsForEachSeed = 10; // numberOfGamesForEachIteration = NumberOfTestsForEachSeed*10
        private readonly List<int> GameSizes = new() { 5, 7 };
        private const int IterationStep = 100;
        private readonly IEnumerable<int> IterationsRange = Enumerable.Range(1, 70).Select(i => i * IterationStep);

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

                        WriterHelper.SaveContentToFile(lines, $"hypo5/winratio-iterations-results{gameSize}.txt");
                    }
                }

                WriterHelper.SaveContentToFile(lines, $"hypo5/winratio-iterations-results{gameSize}.txt");
            }
        }

        private double CalculateWinRatioForAlgorithm(Algorithm engine, int iterations, int gameSize)
        {
            int numberOfWins = 0;
            foreach (var seed in _seedIterator)
            {
                RandomSource.SetSeed(seed);
                for (int i = 0; i < NumberOfTestsForEachSeed; i += 2)
                {
                    numberOfWins += RunSimulationsAndCountWins(engine, Algorithm.Heuristic, iterations, gameSize, PlayerColor.White);
                    numberOfWins += RunSimulationsAndCountWins(Algorithm.Heuristic, engine, iterations, gameSize, PlayerColor.Black);
                }
            }

            return numberOfWins / (double)(NumberOfTestsForEachSeed * _seedIterator.Count * 2);
        }

        private int RunSimulationsAndCountWins(Algorithm whiteAlg, Algorithm blackAlg, int iterations, int gameSize, PlayerColor expectedToWin)
        {
            var config = TestHelpers.CreateEnginesConfiguration(gameSize, whiteAlg, blackAlg, iterations, true);
            var gameSimulator = new GameSimulator(config);
            return gameSimulator.RunSimulation() == expectedToWin ? 1 : 0;
        }
    }
}
