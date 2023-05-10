﻿using minihex.engine.Engine.Engines;
using minihex.engine.Model;
using minihex.engine.Models.Enums;

namespace minihex.engine
{
    public static partial class Helpers
    {
        public static BaseEngine? ToBaseEngine(Algorithm algorithm, GameExt game)
        {
            return algorithm switch
            {
                Algorithm.Human => null,
                Algorithm.Heuristic => new HeuristicEngine(game),
                Algorithm.MCTS => new MctsEngine(game),
                _ => new RandomEngine(game),
            };
        }
    }
}
