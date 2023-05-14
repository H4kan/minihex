using minihex.engine.Model.Games;

namespace minihex.engine.Model.Nodes
{
    public class AmafBridgeStateNode : AmafStateNode
    {
        public AmafBridgeStateNode(int nextMove, StateNode? parent = null) : base(nextMove, parent) { }

        protected override StateNode ConstructNode(int nextMove, StateNode? parent = null)
        {
            return new AmafBridgeStateNode(nextMove, parent);
        }

        protected override GameExt ConstructSimulationGame(int size, bool swap)
        {
            return new BridgeGameExt(size, swap);
        }
    }
}
