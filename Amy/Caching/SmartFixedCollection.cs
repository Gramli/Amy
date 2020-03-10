namespace Amy.Caching
{
    public class SmartFixedCollection<K> : UsageFixedCollection<K> where K : class
    {
        public SmartFixedCollection(int length)
            : base(length)
        {
        }

        public void Add(K key)
        {
            if (Contains(key))
                IncreaseUsage(key);
            else if (this._full)
            {
                K keyToRemove = GetKeyToRemove();
                Remove(keyToRemove);
                AddUsage(key);
            }
            else
                AddUsage(key);
        }

        public override void Remove(K key)
        {
            RemoveUsage(key);
        }

        public override bool Contains(K key)
        {
            return ContainsUsage(key);
        }
    }
}
