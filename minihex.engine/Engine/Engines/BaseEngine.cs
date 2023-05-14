using minihex.engine.Model.Games;

namespace minihex.engine.Engine.Engines
{
    public abstract class BaseEngine
    {
        protected GameExt Game { get; set; }


        public BaseEngine(GameExt game)
        {
            Game = game;
        }

        public virtual void Process(int moveNumber)
        {
            Game.MakeMove(this.GetNextMove(moveNumber), moveNumber);
        }

        public abstract int GetNextMove(int moveNumber);
    }
}
