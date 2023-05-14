using minihex.engine.Model.Games;
using minihex.engine.Model.Nodes;

namespace minihex.engine.Engine.Engines
{
    public class MctsAmafBridgeEngine : MctsEngine
    {
        public MctsAmafBridgeEngine(GameExt game, CancellationToken cancellationToken) : base(game, cancellationToken) { }

        protected override StateNode CreateRoot()
        {
            return new AmafBridgeStateNode(0);
        }
    }
}
