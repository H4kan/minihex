namespace minihex.Web.Models.Requests
{
    public class MakeMoveRequest
    {
        public Guid GameId { get; set; }

        public int MoveNumber { get; set; }

        public int FieldIdx { get; set; }
    }

}
