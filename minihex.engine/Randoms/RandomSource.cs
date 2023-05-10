namespace minihex.engine.Randoms
{
    public static class RandomSource
    {
        public static Random Rand => _rand;

        private static Random _rand = new();

        public static void SetSeed(int seed)
        {
            _rand = new Random(seed);
        }
    }
}
