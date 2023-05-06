using minihex.engine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minihex.engine.Engine.Engines
{
    public abstract class BaseEngine
    {

        protected GameExt Game { get; set; }
        

        public BaseEngine(GameExt game) {
            this.Game = game;
        }


        public virtual void Process(int moveNumber)
        {
            Game.MakeMove(this.GetNextMove(moveNumber), moveNumber);

        }

        public abstract int GetNextMove(int moveNumber);

    }
}
