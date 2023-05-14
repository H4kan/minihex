using minihex.engine.Model.Enums;
using minihex.engine.Randoms;

namespace minihex.engine.Model
{
    public class BridgeStateNode : StateNode
    {
        public BridgeStateNode(int nextMove, StateNode? parent = null) : base(nextMove, parent)
        {
        }

        protected override StateNode ConstructNode(int nextMove, StateNode? parent = null)
        {
            return new BridgeStateNode(nextMove, parent);
        }


        protected override void PlayoutInternal(GameExt game, int size, out int moveNumber)
        {

            var candidates = new List<(int, int)>();

            var avMoves = GetAvailableMoves(size).OrderBy(x => RandomSource.Rand.Next()).ToList();

            moveNumber = 0;
            while (!game.IsFinished(moveNumber + moves.Count))
            {
                if (IsBridged(game, moveNumber + moves.Count, out var saveBridgeMove))
                {
                    game.MakeMove(saveBridgeMove, ++moveNumber + moves.Count, true);
                    avMoves.Remove(saveBridgeMove);
                }
                else
                {
                    game.MakeMove(avMoves[0], ++moveNumber + moves.Count, true);
                    avMoves.Remove(avMoves[0]);
                }
            }
        }

        private bool IsBridged(GameExt game, int moveNumber, out int saveBridgeMove)
        {
            saveBridgeMove = 0;

            var bridgedList = new List<int>();

            if (moveNumber < 2) return false;

            var nextPlayer = moveNumber % 2 == 0 ? PlayerColor.White : PlayerColor.Black;

            var lastMove = game.GetMove(moveNumber);

            var row = lastMove / game.Size;
            var col = lastMove % game.Size;

            var existTopLeft = row > 0;
            
            var existRight = (col < game.Size - 1);
            var existLeft = col > 0;

            var existTopRight = existTopLeft && existRight;
            var existBottomRight = row < game.Size - 1;

            var existBottomLeft = existLeft && existBottomRight; 
           
            if (existLeft && existTopRight && existTopLeft)
            {
                // top left
                if (game.GetColor(row, col - 1) == nextPlayer &&
                    game.GetColor(row - 1, col + 1) == nextPlayer
                    && game.GetColor(row - 1, col) == PlayerColor.None)
                {
                    bridgedList.Add((row - 1) * game.Size + col);
                }
            }

            if (existTopLeft && existRight && existTopRight)
            {
                // top right
                if (game.GetColor(row - 1, col) == nextPlayer &&
                    game.GetColor(row, col + 1) == nextPlayer
                    && game.GetColor(row - 1, col + 1) == PlayerColor.None)
                {
                    bridgedList.Add((row - 1) * game.Size + col + 1);
                }
            }

            if (existTopRight && existBottomRight && existRight)
            {
                // right
                if (game.GetColor(row - 1, col + 1) == nextPlayer &&
                    game.GetColor(row + 1, col) == nextPlayer
                    && game.GetColor(row, col + 1) == PlayerColor.None)
                {
                    bridgedList.Add((row) * game.Size + col + 1);
                }
            }

            if (existRight && existBottomLeft && existBottomRight)
            {
                // bottom right
                if (game.GetColor(row, col + 1) == nextPlayer &&
                    game.GetColor(row + 1, col - 1) == nextPlayer
                    && game.GetColor(row + 1, col) == PlayerColor.None)
                {
                    bridgedList.Add((row + 1) * game.Size + col);
                }
            }

            if (existBottomRight && existLeft && existBottomLeft)
            {
                // bottom left
                if (game.GetColor(row, col - 1) == nextPlayer &&
                    game.GetColor(row + 1, col) == nextPlayer
                    && game.GetColor(row + 1, col - 1) == PlayerColor.None)
                {
                    bridgedList.Add((row + 1) * game.Size + col - 1);
                }
            }

            if (existBottomLeft && existTopLeft && existLeft)
            {
                // left
                if (game.GetColor(row - 1, col) == nextPlayer &&
                    game.GetColor(row + 1, col - 1) == nextPlayer
                    && game.GetColor(row, col - 1) == PlayerColor.None)
                {
                    bridgedList.Add((row) * game.Size + col - 1);
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
