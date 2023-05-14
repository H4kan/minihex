using minihex.engine.Model;
using minihex.engine.Model.Nodes;

namespace minihex.engine.Engine.Engines
{
    public class MctsAmafEngine : MctsEngine
    {
        public MctsAmafEngine(GameExt game, CancellationToken cancellationToken) : base(game, cancellationToken)
        { }

        protected override StateNode CreateRoot()
        {
            return new AmafStateNode(0);
        }
    }
}
