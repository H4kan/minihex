using minihex.engine.Model;

namespace minihex.engine.Engine.Engines
{
    public class MctsEngine : BaseEngine
    {
        private static readonly int MaxIteration = 100000;
        private StateNode? _root;
        private CancellationToken _cancellationToken;

        public MctsEngine(GameExt game, CancellationToken cancellationToken) : base(game) 
        { 
            this._cancellationToken = cancellationToken;
        }

        private void ConductIteration()
        {
            var selection = _root!.Traverse();
            var expansion = selection.Expand(Game.Size);

            expansion.Playout(Game.Size);
        }

        private void ConductAlgorithm(List<int> preMoves, CancellationToken cancellationToken)
        {
            int i = 0;
            _root = new StateNode(0);
            _root.PrependMoves(preMoves);

            while (i++ < MaxIteration)
            {
                ConductIteration();
                cancellationToken.ThrowIfCancellationRequested();
            }
        }

        public override int GetNextMove(int moveNumber)
        {
            var preMoves = Game.GetPreMoves(moveNumber);
            ConductAlgorithm(preMoves, this._cancellationToken);

            return _root!.FetchBestMove().moves.Last();
        }
    }
}
