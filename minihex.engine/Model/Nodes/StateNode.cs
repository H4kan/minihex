using minihex.engine.Model.Enums;
using minihex.engine.Model.Games;
using minihex.engine.Randoms;

namespace minihex.engine.Model.Nodes
{
    public class StateNode
    {
        private static readonly double Exploration = Math.Sqrt(2);

        public double DecidingValue = 0.0;
        public int VisitCount = 0;
        public int WinCount = 0;

        public double WinningRatio => VisitCount == 0 && Parent is not null ? 0.5 * (1 - Parent.WinningRatio) : (double)WinCount / VisitCount;

        public List<int> moves = new();
        public bool IsTerminal = true;
        public bool GameFinished = false;

        public List<StateNode> Children { get; }
        public StateNode? Parent { get; set; }

        protected bool Swap { get; set; }

        public StateNode(int nextMove, bool swap, StateNode? parent = null)
        {
            Children = new List<StateNode>();
            Parent = parent;
            this.Swap = swap;
            if (Parent != null)
            {
                moves.AddRange(Parent.moves);
                moves.Add(nextMove);
            }
            UpdateDecidingValue();
        }

        public virtual void UpdateDecidingValue()
        {
            if (Parent != null)
            {
                if (VisitCount == 0)
                {
                    DecidingValue = double.PositiveInfinity;
                }
                else
                {
                    DecidingValue = (double)WinCount / VisitCount
                        + Exploration * Math.Sqrt(Math.Log(Parent.VisitCount) / VisitCount);
                }
            }
        }

        public StateNode FetchBestMove()
        {
            if (IsTerminal)
            {
                return this;
            }
            else
            {
                return Children.OrderByDescending(c => c.WinningRatio).ToList().First();
            }
        }

        public void PrependMoves(List<int> premoves)
        {
            moves.AddRange(premoves);
        }

        public StateNode Traverse()
        {
            if (IsTerminal) return this;

            return Children.Where(c => !c.GameFinished)
                .OrderByDescending(c => c.DecidingValue).First().Traverse();
        }

        // returns random child
        public StateNode Expand(int size)
        {
            IsTerminal = false;
            var avMoves = GetAvailableMoves(size);

            for (int i = 0; i < avMoves.Count; i++)
            {
                Children.Add(ConstructNode(avMoves[i], this));
            }

            var childIdx = RandomSource.Rand.Next(0, Children.Count);
            return Children[childIdx];
        }

        protected virtual StateNode ConstructNode(int nextMove, StateNode? parent = null)
        {
            return new StateNode(nextMove, this.Swap, parent);
        }

        protected virtual GameExt ConstructSimulationGame(int size, bool swap)
        {
            return new GameExt(size, swap);
        }

        public void Playout(int size)
        {
            var game = ConstructSimulationGame(size, this.Swap);

            for (int i = 0; i < moves.Count - 1; i++)
            {
                MakeMove(game, moves[i], i + 1);
            }
            if (game.IsFinished(moves.Count - 1))
            {
                Parent!.GameFinished = true;
                Parent.WinCount = 0;
                Parent.VisitCount = 0;
                Parent.BackPropagate(!ShouldUpdateWin(game.WhoWon()), game.Moves);
                Parent.BackPropagateValue();
                Parent.IsTerminal = true;
                return;
            }
            else
            {
                MakeMove(game, moves[^1], moves.Count);
            }

            PlayoutInternal(game, size, out int moveNumber);
            GameFinished = moveNumber == 0;

            var winningColor = game.WhoWon();
            var shouldUpdateWin = ShouldUpdateWin(winningColor);

            BackPropagate(shouldUpdateWin, game.Moves);
            BackPropagateValue();
        }

        protected virtual void MakeMove(GameExt game, int fieldIdx, int moveNumber)
        {
            game.MakeMove(fieldIdx, moveNumber, true);
        }

        private bool ShouldUpdateWin(PlayerColor color)
        {
            return color == PlayerColor.White && moves.Count % 2 == 1
                || color == PlayerColor.Black && moves.Count % 2 == 0;
        }

        public virtual void BackPropagate(bool shouldUpdateWin, List<int> moves)
        {
            VisitCount++;
            if (shouldUpdateWin) WinCount++;
            Parent?.BackPropagate(!shouldUpdateWin, moves);
        }

        public void BackPropagateValue()
        {
            UpdateDecidingValue();
            Parent?.BackPropagateValue();
        }

        private void PlayoutInternal(GameExt game, int size, out int moveNumber)
        {
            var avMoves = GetAvailableMoves(size);
            int firstIdx = -1;
            if (this.Swap && moves.Count == 1)
            {
                firstIdx = avMoves[0];
                avMoves.RemoveAt(0);
            }
            avMoves = avMoves.OrderBy(x => RandomSource.Rand.Next()).ToList();
            
            if (firstIdx >= 0)
            {
                if (RandomSource.Rand.NextDouble() > 0.3)
                {
                    avMoves.Insert(0, firstIdx);
                }
            }
            

            game.SimulatePlayout(size, out moveNumber, moves.Count, avMoves);
        }

        protected virtual List<int> GetAvailableMoves(int size)
        {
            var avMoves = Enumerable.Range(0, size * size).Except(moves).ToList();
            if (this.Swap && moves.Count == 1)
            {
                avMoves.Insert(0, moves[0]);
            }
            return avMoves;
        }
    }
}
