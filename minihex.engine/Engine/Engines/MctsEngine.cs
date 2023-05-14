using minihex.engine.Model.Games;
using minihex.engine.Model.Nodes;

namespace minihex.engine.Engine.Engines
{
    public class MctsEngine : BaseEngine
    {
        protected virtual int MaxIteration => 5000;
        protected StateNode? _root;
        private readonly CancellationToken _cancellationToken;

        public MctsEngine(GameExt game, CancellationToken cancellationToken) : base(game)
        {
            _cancellationToken = cancellationToken;
        }

        private void ConductIteration()
        {
            var selection = _root!.Traverse();
            var expansion = selection.Expand(Game.Size);

            expansion.Playout(Game.Size);
        }

        protected virtual StateNode CreateRoot()
        {
            return new StateNode(0);
        }

        private void ConductAlgorithm(List<int> preMoves, CancellationToken cancellationToken)
        {
            int i = 0;
            _root = CreateRoot();
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
            ConductAlgorithm(preMoves, _cancellationToken);

            return _root!.FetchBestMove().moves.Last();
        }
    }
}
