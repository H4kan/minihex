using Microsoft.AspNetCore.Mvc;

namespace Project1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {

        [HttpPost("beginGame")]
        public string BeginGame()
        {
            return Guid.NewGuid().ToString();
        }

        [HttpGet("getMove")]
        public string GetMove()
        {
            return Guid.NewGuid().ToString();
        }

        [HttpPost("makeMove")]
        public string MakeMove()
        {
            return Guid.NewGuid().ToString();
        }

        [HttpGet("getWinningPath")]
        public string GetWinningPath()
        {
            return Guid.NewGuid().ToString();
        }
    }
}