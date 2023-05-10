using minihex.engine.Model;

namespace minihex.engine.Engine.Engines
{
    public class MctsEngine : BaseEngine
    {
        private static readonly int MaxIteration = 100000;
        private StateNode? _root;

        public MctsEngine(GameExt game) : base(game) { }

        private void ConductIteration()
        {
            var selection = _root!.Traverse();
            var expansion = selection.Expand(Game.Size);

            expansion.Playout(Game.Size);
        }

        private void ConductAlgorithm(List<int> preMoves)
        {
            int i = 0;
            _root = new StateNode(0);
            _root.PrependMoves(preMoves);

            while (i++ < MaxIteration)
            {
                ConductIteration();
            }
        }

        public override int GetNextMove(int moveNumber)
        {
            var preMoves = Game.GetPreMoves(moveNumber);
            ConductAlgorithm(preMoves);

            return _root!.FetchBestMove().moves.Last();
        }
    }
}
