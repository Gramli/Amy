namespace Amy.Caching
{
    /// <summary>
    /// Support object for caching
    /// It could be represent by Tuple, but it looks better :)
    /// </summary>
    internal class CacheItem<T>
    {
        public T Value { get; }
        public bool Condition { get; }

        public CacheItem(T value, bool condition)
        {
            this.Value = value;
            this.Condition = condition;
        }

    }
}
