using minihex.engine.Model.Enums;
using minihex.engine.Models.Enums;
using minihex.engine.Randoms;
using minihex.engine.test.Helpers;
using minihex.engine.test.Models;

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
        private const int NumberOfTestsForEachSeed = 2;
        private readonly List<int> GameSizes = new() { 5, 7, 9, 11 };
        private const int IterationStep = 50;
        private readonly IEnumerable<int> IterationsRange = Enumerable.Range(1, 10).Select(i => i * IterationStep);

        [TestMethod]
        public void RunTests()
        {
            foreach (var gameSize in GameSizes)
            {
                var lines = new List<string>() { "Algorithm Iterations WinRatio" };

                foreach (var engine in EnginesToCheck())
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
            for (int i = 0; i < NumberOfTestsForEachSeed; i += 2)
            {
                numberOfWins += RunSimulationsAndCountWins(engine, Algorithm.Heuristic, iterations, gameSize, PlayerColor.White);
                numberOfWins += RunSimulationsAndCountWins(Algorithm.Heuristic, engine, iterations, gameSize, PlayerColor.Black);
            }

            return numberOfWins / (double)(NumberOfTestsForEachSeed * _seedIterator.Count);
        }

        private int RunSimulationsAndCountWins(Algorithm whiteAlg, Algorithm blackAlg, int iterations, int gameSize, PlayerColor expectedToWin)
        {
            int wins = 0;
            foreach (var seed in _seedIterator)
            {
                RandomSource.SetSeed(seed);
                var config = CreateEnginesConfiguration(gameSize, whiteAlg, blackAlg, iterations);
                var gameSimulator = new GameSimulator(config);
                wins += gameSimulator.RunSimulation() == expectedToWin ? 1 : 0;
            }

            return wins;
        }

        public static EnginesSetupConfiguration CreateEnginesConfiguration(int gameSize, Algorithm engine1, Algorithm engine2, int? maxIterations)
        {
            return new EnginesSetupConfiguration
            {
                Engine1 = engine1,
                Engine2 = engine2,
                GameSize = gameSize,
                GameSwapMode = true,
                MaxIterations1 = maxIterations,
                MaxIterations2 = maxIterations
            };
        }

        public static IEnumerable<Algorithm> EnginesToCheck()
        {
            yield return Algorithm.MCTS;
            yield return Algorithm.MCTSwSavebridge;
            yield return Algorithm.MSTSwAMAF;
            yield return Algorithm.MCTSwAMAFandSavebridge;
        }
    }
}
