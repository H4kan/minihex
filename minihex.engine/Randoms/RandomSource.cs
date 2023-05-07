
namespace minihex.engine.Randoms
{
    public static class RandomSource
    {
        public static Random rand = new Random();

        public static void SetSeed(int seed)
        {
            rand = new Random(seed);
        }

    }
}
