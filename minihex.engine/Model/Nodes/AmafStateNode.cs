namespace minihex.engine.Model.Nodes
{
    public class AmafStateNode : StateNode
    {
        public AmafStateNode(int nextMove, StateNode? parent = null) : base(nextMove, parent)
        { }

        public override void BackPropagate(bool shouldUpdateWin, List<int> moves)
        {
            if (!IsTerminal)
            {
                foreach (var child in Children)
                {
                    if (!child.IsTerminal && child.moves.Last() != moves[child.moves.Count - 1])
                    {
                        AmafUpdate(child, moves, !shouldUpdateWin);
                    }
                }
            }

            base.BackPropagate(shouldUpdateWin, moves);
        }

        protected override StateNode ConstructNode(int nextMove, StateNode? parent = null)
        {
            return new AmafStateNode(nextMove, parent);
        }

        private static void AmafUpdate(StateNode node, List<int> moves, bool shouldUpdateWin)
        {
            if (ContainsMove(moves, node.moves.Last(), node.moves.Count + 1))
            {
                node.VisitCount++;
                if (shouldUpdateWin) node.WinCount++;
                node.UpdateDecidingValue();
            }
        }

        private static bool ContainsMove(List<int> moves, int value, int startIndex)
        {
            for (int i = startIndex; i < moves.Count; i += 2)
            {
                if (moves[i] == value)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
