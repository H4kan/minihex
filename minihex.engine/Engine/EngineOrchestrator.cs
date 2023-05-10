using minihex.engine.Engine.Engines;
using minihex.engine.Model;
using minihex.engine.Model.Requests;
using minihex.engine.Randoms;

namespace minihex.engine.Engine
{
    public class EngineOrchestrator
    {
        public GameExt Game { get; set; } = new GameExt(0, false);
        public Guid GameId { get; set; }
        public static EngineOrchestrator Instance => _instance;

        private List<bool> _readyList = Enumerable.Empty<bool>().ToList();
        private BaseEngine? _engine1;
        private BaseEngine? _engine2;
        private int _moveNumber;
        private CancellationTokenSource? _engineProcessTokenSource;

        private static readonly EngineOrchestrator _instance = new();

        private EngineOrchestrator() => RandomSource.SetSeed(0);

        public void SetupEngines(SetupEngineRequest request)
        {
            _engineProcessTokenSource?.Cancel();
            _readyList = Enumerable.Range(0, request.Size * request.Size)
                .Select(i => false).ToList();

            Game = new GameExt(request.Size, request.Swap);
            GameId = Guid.NewGuid();

            _engine1 = Helpers.ToBaseEngine(request.Engine1, Game);
            _engine2 = Helpers.ToBaseEngine(request.Engine2, Game);

            if (_engine1 != null && _engine2 != null)
            {
                StartBothEngineProcessing();
            }
            else if (_engine1 != null)
            {
                StartEngine1Processing();
            }
            else
            {
                StartEngine2Processing();
            }
        }


        public void StartBothEngineProcessing()
        {
            _engineProcessTokenSource = new CancellationTokenSource();
            Task.Run(() =>
            {
                while (!Game.IsFinished(_moveNumber))
                {
                    _engine1!.Process(++_moveNumber);
                    SetReady(_moveNumber);
                    _engineProcessTokenSource.Token.ThrowIfCancellationRequested();
                    if (!Game.IsFinished(_moveNumber))
                    {
                        _engine2!.Process(++_moveNumber);
                        SetReady(_moveNumber);
                        _engineProcessTokenSource.Token.ThrowIfCancellationRequested();
                    }
                }
            }, _engineProcessTokenSource.Token);
        }

        public void StartEngine1Processing()
        {
            _engineProcessTokenSource = new CancellationTokenSource();
            Task.Run(() =>
            {
                while (!Game.IsFinished(_moveNumber))
                {
                    _engine1!.Process(++_moveNumber);
                    SetReady(_moveNumber);
                    _engineProcessTokenSource.Token.ThrowIfCancellationRequested();

                    if (!Game.IsFinished(_moveNumber))
                    {
                        WaitTillReady(++_moveNumber, true);
                    }
                }
            }, _engineProcessTokenSource.Token);
        }

        public void StartEngine2Processing()
        {
            _engineProcessTokenSource = new CancellationTokenSource();
            Task.Run(() =>
            {
                while (!Game.IsFinished(_moveNumber))
                {
                    WaitTillReady(++_moveNumber, true);
                    if (!Game.IsFinished(_moveNumber))
                    {
                        _engine2!.Process(++_moveNumber);
                        SetReady(_moveNumber);
                        _engineProcessTokenSource.Token.ThrowIfCancellationRequested();
                    }
                }
            }, _engineProcessTokenSource.Token);
        }

        public void WaitTillReady(int moveNumber, bool engineSide = false)
        {
            while (!IsReady(moveNumber))
            {
                Thread.Sleep(300);
                if (engineSide) _engineProcessTokenSource?.Token.ThrowIfCancellationRequested();
            }
        }

        private bool IsReady(int moveNumber)
        {
            return _readyList[moveNumber - 1];
        }

        public void SetReady(int moveNumber)
        {
            _readyList[moveNumber - 1] = true;
        }
    }
}
