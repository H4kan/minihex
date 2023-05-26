using minihex.engine.Engine.Engines;
using minihex.engine.Model.Enums;
using minihex.engine.Model.Games;
using minihex.engine.test.Models;

namespace minihex.engine.test.Helpers
{
    public class GameSimulator
    {
        public GameExt Game => _game;

        private readonly BaseEngine? _engine1;
        private readonly BaseEngine? _engine2;
        private readonly GameExt _game;

        public GameSimulator(EnginesSetupConfiguration config)
        {
            _game = new GameExt(config.GameSize, config.GameSwapMode);
            _engine1 = engine.Helpers.ToBaseEngine(config.Engine1, _game, CancellationToken.None, config.MaxIterations1);
            _engine2 = engine.Helpers.ToBaseEngine(config.Engine2, _game, CancellationToken.None, config.MaxIterations1);
        }


        public PlayerColor RunSimulation()
        {
            int moveNumber = 0;

            if (_engine2 is null || _engine1 is null)
                throw new InvalidOperationException();

            while (!_game.IsFinished(moveNumber))
            {
                if (moveNumber % 2 == 0)
                    _engine1!.Process(++moveNumber);
                else
                    _engine2!.Process(++moveNumber);
            }

            return _game.WhoWon();
        }
    }
}
