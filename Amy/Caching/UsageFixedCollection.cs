using System;
using System.Collections.Generic;
using System.Linq;

namespace Amy.Caching
{
    public abstract class UsageFixedCollection<K> where K : class
    {
        private Dictionary<K, int> _usage;
        private int _length;

        protected bool _full { get { return this._usage.Count == this._length; } }

        public UsageFixedCollection(int length)
        {
            this._length = length;
            this._usage = new Dictionary<K, int>();
        }

        protected K GetKeyToRemove()
        {
            return this._usage.OrderBy(x => x.Value).First().Key;
        }

        protected void RemoveUsage(K key)
        {
            this._usage.Remove(key);
        }

        protected void AddUsage(K key)
        {
            if (this._full)
                throw new ArgumentException("Can't add item because of fixed length of collection.");
            else if (ContainsUsage(key))
                IncreaseUsage(key);
            else
                this._usage.Add(key, 0);
        }

        protected void IncreaseUsage(K key)
        {
            this._usage[key]++;
        }

        protected bool ContainsUsage(K key)
        {
            return this._usage.ContainsKey(key);
        }

        public abstract void Remove(K key);

        public abstract bool Contains(K key);
    }
}
