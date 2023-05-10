using minihex.engine.Model.Enums;

namespace minihex.engine.Model
{
    public class GameExt : Game
    {
        private bool _isFinished = false;

        public GraphRepresentation _redWhiteRepresentation;
        public GraphRepresentation _redBlackRepresentation;

        private readonly GraphRepresentation _whiteRepresentation;
        private readonly GraphRepresentation _blackRepresentation;

        public GameExt(int size, bool swap) : base(size, swap)
        {
            _redWhiteRepresentation = new GraphRepresentation(size, PlayerColor.White);
            _redBlackRepresentation = new GraphRepresentation(size, PlayerColor.Black);

            _whiteRepresentation = new GraphRepresentation(size, PlayerColor.White);
            _blackRepresentation = new GraphRepresentation(size, PlayerColor.Black);
        }

        public bool IsFinished(int moveNumber)
        {
            return _isFinished && _moves.Count == moveNumber;
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
    }
}