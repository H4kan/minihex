using minihex.Web.Models.Enums;

namespace minihex.Web.Models.Responses
{
    public class GetWinnigPathResponse
    {
        public Guid GameId { get; set; }

        public PlayerColor ColorWon { get; set; }

        public IEnumerable<int>? Path { get; set; }
    }
}
