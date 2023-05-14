using minihex.engine.Model.Enums;
using minihex.engine.Randoms;

namespace minihex.engine.Model.Games
{
    public class BridgeGameExt : GameExt
    {
        public BridgeGameExt(int size, bool swap) : base(size, swap) { }


        public override void SimulatePlayout(int size, out int moveNumber, int prevMovesCounter, List<int> avMoves)
        {
            moveNumber = 0;
            while (!IsFinished(moveNumber + prevMovesCounter))
            {
                if (IsBridged(moveNumber + prevMovesCounter, out var saveBridgeMove))
                {
                    MakeMove(saveBridgeMove, ++moveNumber + prevMovesCounter, true);
                    avMoves.Remove(saveBridgeMove);
                }
                else
                {
                    MakeMove(avMoves[0], ++moveNumber + prevMovesCounter, true);
                    avMoves.Remove(avMoves[0]);
                }
            }
        }

        private bool IsBridged(int moveNumber, out int saveBridgeMove)
        {
            saveBridgeMove = 0;
            var bridgedList = new List<int>();

            if (moveNumber < 2)
                return false;

            var nextPlayer = moveNumber % 2 == 0 ? PlayerColor.White : PlayerColor.Black;

            var lastMove = GetMove(moveNumber);

            var row = lastMove / Size;
            var col = lastMove % Size;

            var existTopLeft = row > 0;

            var existRight = col < Size - 1;
            var existLeft = col > 0;

            var existTopRight = existTopLeft && existRight;
            var existBottomRight = row < Size - 1;

            var existBottomLeft = existLeft && existBottomRight;

            if (existLeft && existTopRight && existTopLeft)
            {
                // top left
                if (GetColor(row, col - 1) == nextPlayer &&
                    GetColor(row - 1, col + 1) == nextPlayer
                    && GetColor(row - 1, col) == PlayerColor.None)
                {
                    bridgedList.Add((row - 1) * Size + col);
                }
            }

            if (existTopLeft && existRight && existTopRight)
            {
                // top right
                if (GetColor(row - 1, col) == nextPlayer &&
                    GetColor(row, col + 1) == nextPlayer
                    && GetColor(row - 1, col + 1) == PlayerColor.None)
                {
                    bridgedList.Add((row - 1) * Size + col + 1);
                }
            }

            if (existTopRight && existBottomRight && existRight)
            {
                // right
                if (GetColor(row - 1, col + 1) == nextPlayer &&
                    GetColor(row + 1, col) == nextPlayer
                    && GetColor(row, col + 1) == PlayerColor.None)
                {
                    bridgedList.Add(row * Size + col + 1);
                }
            }

            if (existRight && existBottomLeft && existBottomRight)
            {
                // bottom right
                if (GetColor(row, col + 1) == nextPlayer &&
                    GetColor(row + 1, col - 1) == nextPlayer
                    && GetColor(row + 1, col) == PlayerColor.None)
                {
                    bridgedList.Add((row + 1) * Size + col);
                }
            }

            if (existBottomRight && existLeft && existBottomLeft)
            {
                // bottom left
                if (GetColor(row, col - 1) == nextPlayer &&
                    GetColor(row + 1, col) == nextPlayer
                    && GetColor(row + 1, col - 1) == PlayerColor.None)
                {
                    bridgedList.Add((row + 1) * Size + col - 1);
                }
            }

            if (existBottomLeft && existTopLeft && existLeft)
            {
                // left
                if (GetColor(row - 1, col) == nextPlayer &&
                    GetColor(row + 1, col - 1) == nextPlayer
                    && GetColor(row, col - 1) == PlayerColor.None)
                {
                    bridgedList.Add(row * Size + col - 1);
                }
            }

            if (bridgedList.Count > 0)
            {
                saveBridgeMove = bridgedList.OrderBy(x => RandomSource.Rand.Next()).First();
                return true;
            }

            return false;
        }
    }
}
