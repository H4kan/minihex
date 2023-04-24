using Microsoft.AspNetCore.Mvc;
using minihex.Web.Models.Requests;
using minihex.Web.Models.Responses;
using minihex.Web.Models.Enums;
using minihex.Web.Models;

namespace Project1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        // returns new, non-repeatable gameId
        [HttpPost("beginGame")]
        public GameIdentificator BeginGame(BeginGameRequest request)
        {
            return new GameIdentificator()
            {
                GameId = Guid.NewGuid(),
            };
        }

        [HttpPost("getMove")]
        public MoveInfoResponse GetMove(GetMoveRequest request)
        {
            return new MoveInfoResponse()
            {
                FieldIdx = request.MoveNumber,
                Status = request.MoveNumber < 5 ? GameStatus.InProgress : GameStatus.Finished,
                GameId = request.GameId
            };
        }

        [HttpPost("makeMove")]
        public MoveInfoResponse MakeMove(MakeMoveRequest request)
        {
            return new MoveInfoResponse()
            {
                FieldIdx = request.FieldIdx,
                Status = request.MoveNumber < 5 ? GameStatus.InProgress : GameStatus.Finished,
                GameId = request.GameId
            };
        }

        [HttpPost("getWinningPath")]
        public GetWinnigPathResponse GetWinningPath(GameIdentificator request)
        {
            return new GetWinnigPathResponse()
            {
                GameId = request.GameId,
                ColorWon = PlayerColor.White,
                Path = new List<int>() { 1, 2, 3, 4, 5 }
            };
        }
    }
}