using minihex.engine.Model.Enums;

namespace minihex.engine.Model
{
    public class Game
    {

        public PlayerColor[,] board;

        protected List<int> _moves;

        public int Size;

        public Game(int size, bool swap)
        {
            this.board = new PlayerColor[size, size];
            this._moves = new List<int>();
            this.Size = size;
        }


        public virtual void MakeMove(int fieldIdx, int moveNumber)
        {
            if (_moves.Count == moveNumber - 1)
            {
                this.board[fieldIdx / Size, fieldIdx % Size] = 
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