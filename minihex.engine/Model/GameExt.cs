using minihex.engine.Model.Enums;

namespace minihex.engine.Model
{
    public class GameExt : Game
    {

        private bool _isFinished = false;

        public GraphRepresentation _redWhiteRepresentation;
        public GraphRepresentation _redBlackRepresentation;

        private GraphRepresentation _whiteRepresentation;
        private GraphRepresentation _blackRepresentation;

        public GameExt(int size, bool swap): base(size, swap) {
            this._redWhiteRepresentation = new GraphRepresentation(size, PlayerColor.White);
            this._redBlackRepresentation = new GraphRepresentation(size, PlayerColor.Black);

            this._whiteRepresentation = new GraphRepresentation(size, PlayerColor.White);
            this._blackRepresentation = new GraphRepresentation(size, PlayerColor.Black);
        }


        public bool IsFinished(int moveNumber)
        {
            return _isFinished && base._moves.Count == moveNumber;
        }


        public override void MakeMove(int fieldIdx, int moveNumber)
        {
            base.MakeMove(fieldIdx, moveNumber);

            this._redWhiteRepresentation.ColorVerticeAndReduce(fieldIdx, moveNumber % 2 == 0 ? PlayerColor.Black : PlayerColor.White);
            this._redBlackRepresentation.ColorVerticeAndReduce(fieldIdx, moveNumber % 2 == 0 ? PlayerColor.Black : PlayerColor.White);

            this._whiteRepresentation.ColorVertice(fieldIdx, moveNumber % 2 == 0 ? PlayerColor.Black : PlayerColor.White);
            this._blackRepresentation.ColorVertice(fieldIdx, moveNumber % 2 == 0 ? PlayerColor.Black : PlayerColor.White);

            this._isFinished = this._redWhiteRepresentation.IsGameFinished() || this._redBlackRepresentation.IsGameFinished();
        }

        public (List<int>, PlayerColor) GetWinningPath()
        {
            var winningColor = this._redWhiteRepresentation.IsGameFinished() ? PlayerColor.White : PlayerColor.Black;

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