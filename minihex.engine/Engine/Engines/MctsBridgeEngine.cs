using minihex.engine.Model;

namespace minihex.engine.Engine.Engines
{
    public class MctsBridgeEngine : MctsEngine
    {
        protected override int MaxIteration => 5000;

        public MctsBridgeEngine(GameExt game, CancellationToken cancellationToken) : base(game, cancellationToken) 
        { }

        protected override StateNode CreateRoot()
        {
            return new BridgeStateNode(0);
        }
    }
}
