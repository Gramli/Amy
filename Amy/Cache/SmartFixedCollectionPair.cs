using System.Collections.Generic;
using System.Linq;

namespace Amy.Cache
{
    public class SmartFixedCollectionPair<K, V> : UsageFixedCollection<K> where K : class
    {
        public V this[K i]
        {
            get
            {
                IncreaseUsage(i);
                return this._data[i];
            }
        }

        private Dictionary<K, V> _data;

        public SmartFixedCollectionPair(int length)
            : base(length)
        {
            this._data = new Dictionary<K, V>(length);
        }

        public void Add(K key, V value)
        {
            var keyToRemove = GetKeyToRemove();
            if (keyToRemove != null)
            {
                Remove(keyToRemove);
            }
            AddUsage(key);
            this._data.Add(key, value);
        }

        public override void Remove(K key)
        {
            RemoveUsage(key);
            this._data.Remove(key);
        }

        public override bool Contains(K key)
        {
            return this._data.ContainsKey(key);
        }
    }
}
