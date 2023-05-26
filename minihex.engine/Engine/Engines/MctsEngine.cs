using minihex.engine.Model.Games;
using minihex.engine.Model.Nodes;

namespace minihex.engine.Engine.Engines
{
    public class MctsEngine : BaseEngine
    {
        protected virtual int MaxIteration { get; set; } = 5000;
        protected StateNode? _root;
        private readonly CancellationToken _cancellationToken;

        public MctsEngine(GameExt game, CancellationToken cancellationToken, int? maxIterations = null) : base(game)
        {
            _cancellationToken = cancellationToken;
            MaxIteration = maxIterations ?? MaxIteration;
        }

        private void ConductIteration()
        {
            var selection = _root!.Traverse();
            var expansion = selection.Expand(Game.Size);

            expansion.Playout(Game.Size);
        }

        protected virtual StateNode CreateRoot()
        {
            return new StateNode(0, Game.Swap);
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
