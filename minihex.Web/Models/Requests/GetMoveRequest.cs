namespace minihex.Web.Models.Requests
{
    public class GetMoveRequest
    {
        public Guid GameId { get; set; }

        public int MoveNumber { get; set; }
    }
}
