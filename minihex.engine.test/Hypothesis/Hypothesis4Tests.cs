using minihex.engine.Models.Enums;
using minihex.engine.test.Helpers;
using minihex.engine.test.Models;

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
        private const int NumberOfTestsForEachSeed = 2;
        private readonly List<int> GameSizes = new() { 5, 9};
        private const int IterationStep = 50;
        private readonly IEnumerable<int> IterationsRange = Enumerable.Range(1, 10).Select(i => i * IterationStep);


        [TestMethod]
        public void RunTests()
        {
            foreach (var gameSize in GameSizes)
            {
                var lines = new List<string>() { "Algorithm Iterations WinRatio" };

                //foreach (var engine in EnginesToCheck())
                //{
                //    foreach (var iter in IterationsRange)
                //    {
                //        double winRatio = CalculateWinRatioForAlgorithm(engine, iter, gameSize);
                //        lines.Add($"{engine} {iter} {winRatio}");

                //        WriterHelper.SaveContentToFile(lines, $"hypo5/winratio-iterations-results{gameSize}.txt");
                //    }
                //}

                WriterHelper.SaveContentToFile(lines, $"hypo5/winratio-iterations-results{gameSize}.txt");
            }
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
