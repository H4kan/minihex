using Microsoft.AspNetCore.Mvc;
using minihex.engine.Engine;
using minihex.engine.Model.Requests;
using minihex.Web.Models.Enums;
using minihex.Web.Models.Requests;
using minihex.Web.Models.Responses;

namespace Project1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly EngineOrchestrator _engineOrchestrator = EngineOrchestrator.Instance;

        // returns new, non-repeatable gameId
        [HttpPost("beginGame")]
        public GameIdentificatorResponse BeginGame(BeginGameRequest request)
        {
            _engineOrchestrator.SetupEngines(new SetupEngineRequest()
            {
                Engine1 = request.Player1Variant,
                Engine2 = request.Player2Variant,
                Swap = request.Variant == GameVariant.SWAP,
                Size = request.Size,
            });

            return new GameIdentificatorResponse()
            {
                GameId = _engineOrchestrator.GameId,
            };
        }

        [HttpPost("getMove")]
        public MoveInfoResponse GetMove(GetMoveRequest request)
        {
            _engineOrchestrator.WaitTillReady(request.MoveNumber);

            return new MoveInfoResponse()
            {
                FieldIdx = _engineOrchestrator.Game.GetMove(request.MoveNumber),
                Status = _engineOrchestrator.Game.IsFinished(request.MoveNumber) ? GameStatus.Finished : GameStatus.InProgress,
                GameId = request.GameId
            };
        }

        [HttpPost("makeMove")]
        public MoveInfoResponse MakeMove(MakeMoveRequest request)
        {
            _engineOrchestrator.Game.MakeMove(request.FieldIdx, request.MoveNumber);
            _engineOrchestrator.SetReady(request.MoveNumber);

            return new MoveInfoResponse()
            {
                FieldIdx = request.FieldIdx,
                Status = _engineOrchestrator.Game.IsFinished(request.MoveNumber) ? GameStatus.Finished : GameStatus.InProgress,
                GameId = request.GameId
            };
        }

        [HttpPost("getWinningPath")]
        public GetWinnigPathResponse GetWinningPath(GameIdentificatorResponse request)
        {
            var winningPath = _engineOrchestrator.Game.GetWinningPath();
            return new GetWinnigPathResponse()
            {
                GameId = request.GameId,
                ColorWon = winningPath.Item2 == minihex.engine.Model.Enums.PlayerColor.White ? PlayerColor.White : PlayerColor.Black,
                Path = winningPath.Item1
            };
        }
    }
}