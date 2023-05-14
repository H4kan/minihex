﻿using minihex.engine.Engine.Engines;
using minihex.engine.Model.Games;
using minihex.engine.Models.Enums;

namespace minihex.engine
{
    public static partial class Helpers
    {
        public static BaseEngine? ToBaseEngine(Algorithm algorithm, GameExt game, CancellationToken cancellationToken)
        {
            return algorithm switch
            {
                Algorithm.Human => null,
                Algorithm.Heuristic => new HeuristicEngine(game),
                Algorithm.MCTS => new MctsEngine(game, cancellationToken),
                Algorithm.MCTSwSavebridge => new MctsBridgeEngine(game, cancellationToken),
                Algorithm.MSTSwAMAF => new MctsAmafEngine(game, cancellationToken),
                Algorithm.MCTSwAMAFandSavebridge => new MctsAmafBridgeEngine(game, cancellationToken),
                _ => new RandomEngine(game),
            };
        }
    }
}
