using minihex.engine.Models.Enums;

namespace minihex.engine.Model.Requests
{
    public class SetupEngineRequest
    {
        public Algorithm Engine1 { get; set; }
        public Algorithm Engine2 { get; set; }
        public bool Swap { get; set; }
        public int Size { get; set; }
    }
}
