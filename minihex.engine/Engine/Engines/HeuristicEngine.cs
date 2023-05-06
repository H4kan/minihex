using minihex.engine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minihex.engine.Engine.Engines
{
    public class HeuristicEngine : BaseEngine
    {
        public HeuristicEngine(GameExt game): base(game) { }


        public override int GetNextMove(int moveNumber)
        {
            var whitePath = this.Game._redWhiteRepresentation.FindPathDestructive(true);
            var blackPath = this.Game._redWhiteRepresentation.FindPathDestructive(true);

            return whitePath.Intersect(blackPath).First();
        }
    }
}
