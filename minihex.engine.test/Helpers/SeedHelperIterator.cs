namespace minihex.engine.test.Helpers
{
    public class SeedHelperIterator : IEnumerable<int>
    {
        private readonly int[] _values;
        public int Count => _values.Length;

        public SeedHelperIterator()
        {
            _values = new[] { 4, 22284, 573, 23078, 321456 };
        }

        public IEnumerator<int> GetEnumerator()
        {
            return _values.AsEnumerable().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _values.GetEnumerator();
        }
    }
}
