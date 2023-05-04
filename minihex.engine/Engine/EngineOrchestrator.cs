using minihex.engine.Engine.Engines;
using minihex.engine.Model;
using minihex.engine.Model.Requests;


namespace minihex.engine.Engine
{
    public class EngineOrchestrator 
    {
        public static readonly EngineOrchestrator instance = new EngineOrchestrator();

        public GameExt Game { get; set; }

        
        private List<bool> _readyList;

        private BaseEngine Engine1 { get; set; }
        private BaseEngine Engine2 { get; set; }

        private int _moveNumber {  get; set; }

        private CancellationTokenSource engineProcessTokenSource { get; set; }

        public Guid GameId { get; set; }

        private EngineOrchestrator() { 
        }

        public void SetupEngines(SetupEngineRequest request)
        {
            if (this.engineProcessTokenSource != null)
            {
                this.engineProcessTokenSource.Cancel();
            }

            this.Game = new GameExt(request.Size, request.Swap);

            this._readyList = Enumerable
                .Range(0, request.Size * request.Size).Select(i => false).ToList();

            this._moveNumber = 0;

            this.GameId = Guid.NewGuid();

            this.Engine1 = Helpers.ToBaseEngine(request.Engine1, Game);
            this.Engine2 = Helpers.ToBaseEngine(request.Engine2, Game);


            if (this.Engine1 != null && this.Engine2 != null)
            {
                this.StartBothEngineProcessing();
            }
            else if (this.Engine1 != null)
            {
                this.StartEngine1Processing();
            }
            else
            {
                this.StartEngine2Processing();
            }
        }


        public void StartBothEngineProcessing()
        {
            engineProcessTokenSource = new CancellationTokenSource();
            Task.Run(() =>
            {
                while (!Game.IsFinished())
                {
                    this.Engine1.Process();
                    this.SetReady(++_moveNumber);
                    this.engineProcessTokenSource.Token.ThrowIfCancellationRequested();
                    if (!Game.IsFinished())
                    {
                        this.Engine2.Process();
                        this.SetReady(++_moveNumber);
                        this.engineProcessTokenSource.Token.ThrowIfCancellationRequested();
                    }
                }
            }, engineProcessTokenSource.Token);
        }

        public void StartEngine1Processing()
        {
            engineProcessTokenSource = new CancellationTokenSource();
            Task.Run(() =>
            {
                while (!Game.IsFinished())
                {
         
                    this.Engine1.Process();
                    this.SetReady(++_moveNumber);
                    this.engineProcessTokenSource.Token.ThrowIfCancellationRequested();
                  
                    if (!Game.IsFinished())
                    {
                        this.WaitTillReady(++_moveNumber, true);
                    }
                }
            }, engineProcessTokenSource.Token);
        }

        public void StartEngine2Processing()
        {
            engineProcessTokenSource = new CancellationTokenSource();
            Task.Run(() =>
            {
                while (!Game.IsFinished())
                {
                    this.WaitTillReady(++_moveNumber, true);

                    if (!Game.IsFinished())
                    {
                        this.Engine2.Process();
                        this.SetReady(++_moveNumber);
                        this.engineProcessTokenSource.Token.ThrowIfCancellationRequested();

                    }
                }
            }, engineProcessTokenSource.Token);
        }

        public void WaitTillReady(int moveNumber, bool engineSide = false)
        {
            while (!IsReady(moveNumber))
            {
                Thread.Sleep(300);
                if (engineSide) this.engineProcessTokenSource.Token.ThrowIfCancellationRequested();
            }
        }

        private bool IsReady(int moveNumber)
        {
            return this._readyList[moveNumber - 1];
        }

        public void SetReady(int moveNumber)
        {
            _readyList[moveNumber - 1] = true;
        }
    }
}
