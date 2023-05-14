﻿using minihex.engine.Model.Games;

namespace minihex.engine.Model.Nodes
{
    public class BridgeStateNode : StateNode
    {
        public BridgeStateNode(int nextMove, StateNode? parent = null) : base(nextMove, parent) { }

        protected override StateNode ConstructNode(int nextMove, StateNode? parent = null)
        {
            return new BridgeStateNode(nextMove, parent);
        }

        protected override GameExt ConstructSimulationGame(int size, bool swap)
        {
            return new BridgeGameExt(size, swap);
        }
    }
}
