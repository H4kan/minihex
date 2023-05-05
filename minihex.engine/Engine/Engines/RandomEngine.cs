using minihex.engine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minihex.engine.Engine.Engines
{
    // this exists only for test purposes
    public class RandomEngine : BaseEngine
    {
        public RandomEngine(Game game): base(game) { }


        public override int GetNextMove(int moveNumber)
        {
            var freeList = new List<int>();

            for (int i = 0; i < Game.board.Length; i++)
            {
                if (Game.board[i / Game.Size, i % Game.Size] == Model.Enums.PlayerColor.None)
                    freeList.Add(i);
            }
            

            var rnd = new Random().Next(0, freeList.Count);

            return freeList[rnd];
        }
    }
}
