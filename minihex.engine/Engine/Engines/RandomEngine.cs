using minihex.engine.Model;

namespace minihex.engine.Engine.Engines
{
    // this exists only for test purposes
    public class RandomEngine : BaseEngine
    {
        public RandomEngine(GameExt game) : base(game) { }


        public override int GetNextMove(int moveNumber)
        {
            var freeList = new List<int>();

            for (int i = 0; i < Game.Board.Length; i++)
            {
                if (Game.Board[i / Game.Size, i % Game.Size] == Model.Enums.PlayerColor.None)
                    freeList.Add(i);
            }

            var rnd = new Random().Next(0, freeList.Count);

            return freeList[rnd];
        }
    }
}
