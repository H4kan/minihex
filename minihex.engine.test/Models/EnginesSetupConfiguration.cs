using minihex.engine.Models.Enums;

namespace minihex.engine.test.Models
{
    public class EnginesSetupConfiguration
    {
        public Algorithm Engine1 { get; set; }
        public Algorithm Engine2 { get; set; }
        public int? MaxIterations1 { get; set; }
        public int? MaxIterations2 { get; set; }
        public bool GameSwapMode { get; set; }
        public int GameSize { get; set; }
    }
}
