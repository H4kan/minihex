﻿using minihex.engine.Engine.Engines;
using minihex.engine.Model;
using minihex.engine.Models.Enums;
using System.Security.Cryptography;

namespace minihex.engine
{
    public static partial class Helpers
    {
        public static BaseEngine ToBaseEngine(Algorithm algorithm, Game game)
        {
            switch (algorithm)
            {
                case Algorithm.Human:
                    return null;
                case Algorithm.Heuristic:
                    return new RandomEngine(game);
                case Algorithm.MCTS:
                    return new RandomEngine(game);
                default:
                    return new RandomEngine(game);
            }
        }
    }
}
