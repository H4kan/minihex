using minihex.engine.Model.Enums;

namespace minihex.engine.Model
{
    public class Game
    {

        private PlayerColor[,] _board;

        private List<int> _moves;

        private int _size;

        public Game(int size, bool swap)
        {
            this._board = new PlayerColor[size, size];
            this._moves = new List<int>();
            this._size = size;
        }


        public virtual void MakeMove(int fieldIdx, int moveNumber)
        {
            if (_moves.Count == moveNumber - 1)
            {
                this._board[fieldIdx / _size, fieldIdx % _size] = 
                    moveNumber % 2 == 0 ? PlayerColor.Black : PlayerColor.White;
                _moves.Add(fieldIdx);
            }
        }

        public int GetMove(int moveNumber)
        {
            return _moves[moveNumber - 1];
        }
    }
}