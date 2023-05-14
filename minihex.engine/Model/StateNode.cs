using minihex.engine.Model.Enums;
using minihex.engine.Randoms;
using System.Runtime.CompilerServices;

namespace minihex.engine.Model
{
    public class StateNode
    {
        private static readonly double Exploration = Math.Sqrt(2);

        public double DecidingValue = 0.0;
        public int VisitCount = 0;
        public int WinCount = 0;

        public double WinningRatio => (VisitCount == 0 && this.Parent is not null ? 0.5 * (1 - this.Parent.WinningRatio) : (double)WinCount / VisitCount);

        public List<int> moves = new();
        public bool IsTerminal = true;
        public bool GameFinished = false;

        public List<StateNode> Children { get; }
        public StateNode? Parent { get; set; }

        public StateNode(int nextMove, StateNode? parent = null)
        {
            this.Children = new List<StateNode>();
            this.Parent = parent;
            if (this.Parent != null)
            {
                this.moves.AddRange(this.Parent.moves);
                this.moves.Add(nextMove);
            }
            this.UpdateDecidingValue();
        }

        protected virtual void UpdateDecidingValue()
        {
            if (this.Parent != null)
            {
                if (VisitCount == 0)
                {
                    this.DecidingValue = Double.PositiveInfinity;
                }
                else
                {
                    this.DecidingValue = (double)WinCount / VisitCount
                        + Exploration * Math.Sqrt(Math.Log(this.Parent.VisitCount) / VisitCount);
                }
            }
        }

        public StateNode FetchBestMove()
        {
            if (this.IsTerminal)
            {
                return this;
            }
            else
            {
                return this.Children.OrderByDescending(c => c.WinningRatio).ToList().First();
            }
        }

        public void PrependMoves(List<int> premoves)
        {
            this.moves.AddRange(premoves);
        }

        public StateNode Traverse()
        {
            if (this.IsTerminal) return this;

            return this.Children.Where(c => !c.GameFinished)
                .OrderByDescending(c => c.DecidingValue).First().Traverse();
        }

        // returns random child
        public StateNode Expand(int size)
        {
            this.IsTerminal = false;
            var avMoves = GetAvailableMoves(size);

            for (int i = 0; i < avMoves.Count; i++)
            {
                this.Children.Add(ConstructNode(avMoves[i], this));
            }

            var childIdx = RandomSource.Rand.Next(0, this.Children.Count);
            return this.Children[childIdx];
        }

        protected virtual StateNode ConstructNode(int nextMove, StateNode? parent = null)
        {
            return new StateNode(nextMove, parent);
        }

        public void Playout(int size)
        {
            var game = new GameExt(size, false);

            for (int i = 0; i < moves.Count - 1; i++)
            {
                this.MakeMove(game, moves[i], i + 1);
            }
            if (game.IsFinished(moves.Count - 1))
            {
                this.Parent!.GameFinished = true;
                this.Parent.WinCount = 0;
                this.Parent.VisitCount = 0;
                this.Parent.BackPropagate(!ShouldUpdateWin(game.WhoWon()));
                this.Parent.BackPropagateValue();
                this.Parent.IsTerminal = true;
                return;
            }
            else
            {
                this.MakeMove(game, moves[^1], moves.Count);
            }

            this.PlayoutInternal(game, size, out int moveNumber);
            this.GameFinished = moveNumber == 0;

            var winningColor = game.WhoWon();
            var shouldUpdateWin = ShouldUpdateWin(winningColor);

            BackPropagate(shouldUpdateWin);
            BackPropagateValue();
        }

        protected virtual void MakeMove(GameExt game, int fieldIdx, int moveNumber)
        {
            game.MakeMove(fieldIdx, moveNumber, true);
        }

        private bool ShouldUpdateWin(PlayerColor color)
        {
            return (color == Enums.PlayerColor.White && moves.Count % 2 == 1)
                || (color == Enums.PlayerColor.Black && moves.Count % 2 == 0);
        }

        public void BackPropagate(bool shouldUpdateWin)
        {
            this.VisitCount++;
            if (shouldUpdateWin) this.WinCount++;
            this.Parent?.BackPropagate(!shouldUpdateWin);
        }

        public void BackPropagateValue()
        {
            UpdateDecidingValue();
            Parent?.BackPropagateValue();
        }

        protected virtual void PlayoutInternal(GameExt game, int size, out int moveNumber)
        {
            var avMoves = GetAvailableMoves(size).OrderBy(x => RandomSource.Rand.Next()).ToList();

            moveNumber = 0;
            while (!game.IsFinished(moveNumber + moves.Count))
            {
                game.MakeMove(avMoves[moveNumber], ++moveNumber + moves.Count, true);
            }
        }

        protected virtual List<int> GetAvailableMoves(int size)
        {
            return Enumerable.Range(0, size * size).Except(this.moves).ToList();
        }
    }
}
