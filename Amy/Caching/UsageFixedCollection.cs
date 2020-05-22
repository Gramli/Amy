﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Amy.Caching
{
    /// <summary>
    /// Base for fixed length collection with usage count
    /// Always when item is get its usage increase
    /// </summary>
    public abstract class UsageFixedCollection<K>
    {
        private readonly Dictionary<K, int> _usage;
        private readonly int _length;

        protected bool _full => this._usage.Count == this._length;

        protected UsageFixedCollection(int length)
        {
            this._length = length;
            this._usage = new Dictionary<K, int>();
        }

        protected K GetKeyToRemove()
        {
            //TODO DAN sort items clever
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

        public int Count()
        {
            return this._usage.Count;
        }

        protected IEnumerator<K> GetUsageEnumerator()
        {
            return this._usage.Keys.GetEnumerator();
        }

        protected void ClearUsage()
        {
            this._usage.Clear();
        }

        public abstract bool Remove(K key);

        public abstract bool Contains(K key);
    }
}
