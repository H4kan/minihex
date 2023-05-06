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
        public MctsEngine(GameExt game) : base(game) { }

        public override int GetNextMove(int moveNumber)
        {
            throw new NotImplementedException();
        }
    }
}
