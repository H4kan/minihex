using minihex.engine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minihex.engine.Engine.Engines
{
    public class MctsEngine : BaseEngine
    {

        public static int MaxIteration = 100000;

        private StateNode _root;

        public MctsEngine(GameExt game) : base(game) {

            
        }

        private void ConductIteration()
        {
            var selection = this._root.Traverse();

            var expansion = selection.Expand(Game.Size);

            expansion.Playout(Game.Size);
        }

        private void ConductAlgorithm(List<int> preMoves)
        {
            int i = 0;
            this._root = new StateNode(0);
            this._root.PrependMoves(preMoves);

            while (i++ < MaxIteration)
            {
                this.ConductIteration();
            }
        }

        public override int GetNextMove(int moveNumber)
        {
            var preMoves = this.Game.GetPreMoves(moveNumber);

            this.ConductAlgorithm(preMoves);

            return this._root.FetchBestMove().moves.Last();
        }
    }
}
