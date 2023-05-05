using minihex.engine.Model.Enums;

namespace minihex.engine.Model
{
    public class GameExt : Game
    {

        private bool _isFinished = false;

        private GraphRepresentation _whiteRepresentation;
        private GraphRepresentation _blackRepresentation;

        public GameExt(int size, bool swap): base(size, swap) {
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

            this._whiteRepresentation.ColorVerticeAndReduce(fieldIdx, moveNumber % 2 == 0 ? PlayerColor.Black : PlayerColor.White);
            this._blackRepresentation.ColorVerticeAndReduce(fieldIdx, moveNumber % 2 == 0 ? PlayerColor.Black : PlayerColor.White);

            this._isFinished = this._whiteRepresentation.IsGameFinished() || this._blackRepresentation.IsGameFinished();
        }

        public (List<int>, PlayerColor) GetWinningPath()
        {
            var winningColor = this._whiteRepresentation.IsGameFinished() ? PlayerColor.White : PlayerColor.Black;
            return (new List<int>() { 1, 2, 3, 4, 5 }, winningColor);
        }
    }
}