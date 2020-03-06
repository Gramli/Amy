namespace Amy.Cache
{
    public class SmartFixedCollection<K> : UsageFixedCollection<K> where K : class
    {
        public SmartFixedCollection(int length)
            : base(length)
        {
        }

        public void Add(K key)
        {
            var keyToRemove = GetKeyToRemove();
            if (keyToRemove != null)
            {
                Remove(keyToRemove);
            }
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
