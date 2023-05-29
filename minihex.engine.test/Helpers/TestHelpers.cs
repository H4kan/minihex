using minihex.engine.Models.Enums;
using minihex.engine.test.Models;

namespace minihex.engine.test.Helpers
{
    public static partial class TestHelpers
    {
        public static IEnumerable<(Algorithm, Algorithm)> UniqueAlgorithmPairs()
        {
            var values = Enum.GetValues(typeof(Algorithm)).Cast<Algorithm>().ToList().Skip(1);
            return values.SelectMany(value => values,
                                      (first, second) => (first, second))
                           .Where(pair => !pair.first.Equals(pair.second)).ToList();
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

        public static EnginesSetupConfiguration CreateEnginesConfiguration(int gameSize, Algorithm engine1, Algorithm engine2, int? maxIterations, bool swap)
        {
            return new EnginesSetupConfiguration
            {
                Engine1 = engine1,
                Engine2 = engine2,
                GameSize = gameSize,
                GameSwapMode = swap,
                MaxIterations1 = maxIterations,
                MaxIterations2 = maxIterations
            };
        }

        public static EnginesSetupConfiguration CreateEnginesConfiguration(int gameSize, Algorithm engine1, Algorithm engine2, int? whiteIterations, int? blackIterations, bool swap)
        {
            return new EnginesSetupConfiguration
            {
                Engine1 = engine1,
                Engine2 = engine2,
                GameSize = gameSize,
                GameSwapMode = swap,
                MaxIterations1 = whiteIterations,
                MaxIterations2 = blackIterations
            };
        }

        public static Dictionary<Algorithm, GameStats> InitializeResultsDictionary()
        {
            return GetAllEngines()
                .ToDictionary(algo => algo, algo => new GameStats { GamesWon = 0, GamesPlayed = 0 });
        }

        public static IEnumerable<Algorithm> GetAllEngines()
        {
            return Enum.GetValues(typeof(Algorithm))
                .Cast<Algorithm>().Skip(1);
        }

        public static IEnumerable<Algorithm> GetMCTSEngines()
        {
            yield return Algorithm.MCTS;
            yield return Algorithm.MCTSwSavebridge;
            yield return Algorithm.MSTSwAMAF;
            yield return Algorithm.MCTSwAMAFandSavebridge;
        }
    }
}
