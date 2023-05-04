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

        protected Game Game { get; set; }
        

        public BaseEngine(Game game) {
            this.Game = game;
        }


        public virtual void Process()
        {
         
          

        }

    }
}
