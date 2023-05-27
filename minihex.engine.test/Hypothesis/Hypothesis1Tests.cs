using minihex.engine.Model.Enums;
using minihex.engine.Models.Enums;
using minihex.engine.Randoms;
using minihex.engine.test.Helpers;

namespace minihex.engine.test.Hypothesis
{
    /// <summary>
    /// Siła SI (\% gier wygrywanych z innymi SI i człowiekiem) opartych o MCTS będzie wzrastać logarytmicznie wraz ze wzrostem
    /// liczby iteracji ekspansji MCTS
    /// </summary>
    [TestClass]
    public class Hypothesis1Tests
    {
        private readonly SeedHelperIterator _seedIterator = new();
        private const int NumberOfTestsForEachSeedAndEngine = 1; // numberOfGamesForEachIteration = NumberOfTestsForEachSeedAndEngine*50
        private readonly List<int> GameSizes = new() { 5/*, 9*/ };
        private const int IterationStep = 50;
        private readonly IEnumerable<int> IterationsRange = Enumerable.Range(1, 10).Select(i => i * IterationStep);


        [TestMethod]
        [DataRow(false, "noswap.txt")]
        [DataRow(true, "swap.txt")]
        public void RunTests(bool swap, string fileNamePart)
        {
            foreach (var gameSize in GameSizes)
            {
                var lines = new List<string>() { "Algorithm Iterations WinRatio" };
                foreach (var engine in TestHelpers.GetMCTSEngines())
                {
                    foreach (var iter in IterationsRange)
                    {
                        double winRatio = CalculateWinRatioForAlgorithm(engine, iter, gameSize, swap);
                        lines.Add($"{engine} {iter} {winRatio}");

                        WriterHelper.SaveContentToFile(lines, $"hypo1/winratio-iterations-results-hex{gameSize}-{fileNamePart}");
                    }
                }
                WriterHelper.SaveContentToFile(lines, $"hypo1/winratio-iterations-results-hex{gameSize}-{fileNamePart}");
            }
        }

        private double CalculateWinRatioForAlgorithm(Algorithm engine, int iterations, int gameSize, bool swap)
        {
            int numberOfWins = 0;
            var enemiesEngines = TestHelpers.GetAllEngines();
            for (int i = 0; i < NumberOfTestsForEachSeedAndEngine; i++)
            {
                foreach (var enemy in enemiesEngines)
                {
                    numberOfWins += RunSimulationsAndCountWins(engine, enemy, iterations, gameSize, PlayerColor.White, swap);
                    numberOfWins += RunSimulationsAndCountWins(enemy, engine, iterations, gameSize, PlayerColor.Black, swap);
                }
            }

            return numberOfWins / (double)(NumberOfTestsForEachSeedAndEngine * _seedIterator.Count * enemiesEngines.Count() * 2);
        }

        private int RunSimulationsAndCountWins(Algorithm whiteAlg, Algorithm blackAlg, int iterations, int gameSize, PlayerColor expectedToWin, bool swap)
        {
            int wins = 0;
            foreach (var seed in _seedIterator)
            {
                int? whiteIterations = PlayerColor.White == expectedToWin ? iterations : null;
                int? blackIterations = PlayerColor.Black == expectedToWin ? iterations : null;

                RandomSource.SetSeed(seed);
                var config = TestHelpers.CreateEnginesConfiguration(gameSize, whiteAlg, blackAlg, whiteIterations, blackIterations, swap);
                var gameSimulator = new GameSimulator(config);
                wins += gameSimulator.RunSimulation() == expectedToWin ? 1 : 0;
            }

            return wins;
        }
    }
}
