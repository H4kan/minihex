﻿using minihex.engine.Model.Enums;

namespace minihex.engine.Model
{
    public class Game
    {
        public List<int> Moves => _moves;
        public PlayerColor[,] Board;
        public int Size;

        protected List<int> _moves;

        public Game(int size, bool swap)
        {
            this.Board = new PlayerColor[size, size];
            _moves = new List<int>();
            this.Size = size;
        }


        public virtual void MakeMove(int fieldIdx, int moveNumber, bool optimizeForEngine = false)
        {
            if (_moves.Count == moveNumber - 1)
            {
                this.Board[fieldIdx / Size, fieldIdx % Size] = 
                    moveNumber % 2 == 0 ? PlayerColor.Black : PlayerColor.White;
                _moves.Add(fieldIdx);
            }
        }

        public int GetMove(int moveNumber)
        {
            return _moves[moveNumber - 1];
        }

        public PlayerColor GetColor(int row, int col)
        {
            return Board[row, col];
        }

        public List<int> GetPreMoves(int moveNumber)
        {
            return _moves.Take(moveNumber - 1).ToList();
        }
    }
}