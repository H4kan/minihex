using minihex.Web.Models.Enums;

namespace minihex.Web.Models.Requests
{
    public class BeginGameRequest
    {
        public Algorithm Player1Variant { get; set; }

        public Algorithm Player2Variant { get; set; }

        public GameVariant Variant { get; set; }

        public int Size { get; set; }

        public int Delay { get; set; }
    }



    
}
