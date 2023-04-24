using minihex.Web.Models.Enums;

namespace minihex.Web.Models.Responses
{
    public class MoveInfoResponse
    {
        public int FieldIdx { get; set; }

        public Guid GameId { get; set; }

        public GameStatus Status { get; set; }
    }

}
