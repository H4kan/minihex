using minihex.engine.Model.Games;
using minihex.engine.Model.Nodes;

namespace minihex.engine.Engine.Engines
{
    public class MctsAmafEngine : MctsEngine
    {
        public MctsAmafEngine(GameExt game, CancellationToken cancellationToken, int? maxIterations = null)
            : base(game, cancellationToken, maxIterations) { }

        protected override StateNode CreateRoot()
        {
            return new AmafStateNode(0, Game.Swap);
        }
    }
}
