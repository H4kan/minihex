using minihex.engine.Model.Enums;

namespace minihex.engine.Model.Games
{
    public class GameExt : Game
    {
        private bool _isFinished = false;

        public readonly GraphRepresentation _redWhiteRepresentation;
        public readonly GraphRepresentation _redBlackRepresentation;

        public readonly GraphRepresentation _whiteRepresentation;
        public readonly GraphRepresentation _blackRepresentation;

        public bool Swap { get; private set; }

        public GameExt(int size, bool swap) : base(size)
        {
            this.Swap = swap;
            _redWhiteRepresentation = new GraphRepresentation(size, PlayerColor.White);
            _redBlackRepresentation = new GraphRepresentation(size, PlayerColor.Black, swap);

            _whiteRepresentation = new GraphRepresentation(size, PlayerColor.White);
            _blackRepresentation = new GraphRepresentation(size, PlayerColor.Black, swap);
        }

        public bool IsFinished(int moveNumber)
        {
            return _isFinished && _moves.Count <= moveNumber;
        }

        public override void MakeMove(int fieldIdx, int moveNumber, bool optimizeForEngine = false)
        {
            base.MakeMove(fieldIdx, moveNumber);
            var color = moveNumber % 2 == 0 ? PlayerColor.Black : PlayerColor.White;

            _redWhiteRepresentation.ColorVerticeAndReduce(fieldIdx, color);
            _redBlackRepresentation.ColorVerticeAndReduce(fieldIdx, color);

            if (!optimizeForEngine)
            {
                _whiteRepresentation.ColorVertice(fieldIdx, color);
                _blackRepresentation.ColorVertice(fieldIdx, color);
            }

            _isFinished = _redWhiteRepresentation.IsGameFinished() || _redBlackRepresentation.IsGameFinished();
        }

        public PlayerColor WhoWon()
        {
            return _redWhiteRepresentation.IsGameFinished() ? PlayerColor.White : PlayerColor.Black;
        }

        public (List<int>, PlayerColor) GetWinningPath()
        {
            var winningColor = WhoWon();
            List<int> path;

            if (winningColor == PlayerColor.White)
            {
                path = _whiteRepresentation.FindPathDestructive(false);
            }
            else
            {
                path = _blackRepresentation.FindPathDestructive(false);
            }

            return (path, winningColor);
        }

        public virtual void SimulatePlayout(int size, out int moveNumber, int prevMovesCounter, List<int> avMoves)
        {
            moveNumber = 0;
            while (!IsFinished(moveNumber + prevMovesCounter))
            {
                MakeMove(avMoves[moveNumber], ++moveNumber + prevMovesCounter, true);
            }
        }
    }
}