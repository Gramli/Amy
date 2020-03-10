using System.Collections.Generic;
using System.Linq;

namespace Amy.Caching
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
            if (Contains(key))
                IncreaseUsage(key);
            else if (this._full)
            {
                K keyToRemove = GetKeyToRemove();
                Remove(keyToRemove);
                AddWithUsage(key, value);
            }
            else 
                AddWithUsage(key, value);
        }

        private void AddWithUsage(K key, V value)
        {
            this._data.Add(key, value);
            AddUsage(key);
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
