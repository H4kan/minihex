using minihex.engine.Model.Games;

namespace minihex.engine.Engine.Engines
{
    public class HeuristicEngine : BaseEngine
    {
        public HeuristicEngine(GameExt game) : base(game) { }


        public override int GetNextMove(int moveNumber)
        {
            var whitePath = Game._redWhiteRepresentation.FindPathDestructive(true);
            var blackPath = Game._redBlackRepresentation.FindPathDestructive(true);

            return whitePath.Intersect(blackPath).First();
        }
    }
}
