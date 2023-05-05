using Microsoft.AspNetCore.Mvc;
using minihex.Web.Models.Requests;
using minihex.Web.Models.Responses;
using minihex.Web.Models.Enums;
using minihex.Web.Models;
using minihex.engine.Engine;
using minihex.engine.Model.Requests;

namespace Project1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        
        private EngineOrchestrator _engineOrchestrator = EngineOrchestrator.instance;

        // returns new, non-repeatable gameId
        [HttpPost("beginGame")]
        public GameIdentificator BeginGame(BeginGameRequest request)
        {

            this._engineOrchestrator.SetupEngines(new SetupEngineRequest()
            {
                Engine1 = request.Player1Variant,
                Engine2 = request.Player2Variant,
                Swap = request.Variant == GameVariant.SWAP,
                Size = request.Size,
            });

            return new GameIdentificator()
            {
                GameId = this._engineOrchestrator.GameId,
            };
        }

        [HttpPost("getMove")]
        public MoveInfoResponse GetMove(GetMoveRequest request)
        {
            this._engineOrchestrator.WaitTillReady(request.MoveNumber);

            return new MoveInfoResponse()
            {
                FieldIdx = this._engineOrchestrator.Game.GetMove(request.MoveNumber),
                Status = this._engineOrchestrator.Game.IsFinished(request.MoveNumber) ? GameStatus.Finished : GameStatus.InProgress,
                GameId = request.GameId
            };
        }

        [HttpPost("makeMove")]
        public MoveInfoResponse MakeMove(MakeMoveRequest request)
        {
            this._engineOrchestrator.Game.MakeMove(request.FieldIdx, request.MoveNumber);
            this._engineOrchestrator.SetReady(request.MoveNumber);

            return new MoveInfoResponse()
            {
                FieldIdx = request.FieldIdx,
                Status = this._engineOrchestrator.Game.IsFinished(request.MoveNumber) ? GameStatus.Finished : GameStatus.InProgress,
                GameId = request.GameId
            };
        }

        [HttpPost("getWinningPath")]
        public GetWinnigPathResponse GetWinningPath(GameIdentificator request)
        {
            var winningPath = this._engineOrchestrator.Game.GetWinningPath();
            return new GetWinnigPathResponse()
            {
                GameId = request.GameId,
                ColorWon = winningPath.Item2 == minihex.engine.Model.Enums.PlayerColor.White ? PlayerColor.White : PlayerColor.Black,
                Path = winningPath.Item1
            };
        }
    }
}