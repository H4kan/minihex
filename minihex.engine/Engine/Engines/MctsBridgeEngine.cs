using minihex.engine.Model.Games;
using minihex.engine.Model.Nodes;

namespace minihex.engine.Engine.Engines
{
    public class MctsBridgeEngine : MctsEngine
    {
        protected override int MaxIteration => 5000;
        public MctsBridgeEngine(GameExt game, CancellationToken cancellationToken, int? maxIterations = null) 
            : base(game, cancellationToken, maxIterations) { }

        protected override StateNode CreateRoot()
        {
            return new BridgeStateNode(0, Game.Swap);
        }
    }
}
