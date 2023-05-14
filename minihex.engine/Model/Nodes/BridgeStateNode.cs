using minihex.engine.Model.Games;

namespace minihex.engine.Model.Nodes
{
    public class BridgeStateNode : StateNode
    {
        public BridgeStateNode(int nextMove, bool swap, StateNode? parent = null) : base(nextMove, swap, parent) { }

        protected override StateNode ConstructNode(int nextMove, StateNode? parent = null)
        {
            return new BridgeStateNode(nextMove, this.Swap, parent);
        }

        protected override GameExt ConstructSimulationGame(int size, bool swap)
        {
            return new BridgeGameExt(size, swap);
        }
    }
}
