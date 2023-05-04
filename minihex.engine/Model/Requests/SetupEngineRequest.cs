using minihex.engine.Engine.Engines;
using minihex.engine.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minihex.engine.Model.Requests
{
    public class SetupEngineRequest
    {
        public Algorithm Engine1 { get; set; }

        public Algorithm Engine2 { get; set; }

        public bool Swap { get; set; }

        public int Size { get; set; }

    }
}
