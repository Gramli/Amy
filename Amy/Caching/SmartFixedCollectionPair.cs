using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System;

namespace Amy.Caching
{
    public class SmartFixedCollectionPair<K, V> : UsageFixedCollection<K>, IDictionary<K, V> where K : class
    {
        public V this[K i]
        {
            get
            {
                IncreaseUsage(i);
                return this._data[i];
            }
            set { new InvalidOperationException("Can't set new value, its read only"); }
        }

        private Dictionary<K, V> _data;

        public ICollection<K> Keys => this._data.Keys;

        public ICollection<V> Values => this._data.Values;

        int ICollection<KeyValuePair<K, V>>.Count => this._data.Count;

        public bool IsReadOnly => true;

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

        public override bool Remove(K key)
        {
            RemoveUsage(key);
            this._data.Remove(key);
            return true;
        }

        public override bool Contains(K key)
        {
            return this._data.ContainsKey(key);
        }

        public bool ContainsKey(K key)
        {
            return this.Contains(key);
        }

        public bool TryGetValue(K key, [MaybeNullWhen(false)] out V value)
        {
            throw new System.NotImplementedException();
        }

        public void Add(KeyValuePair<K, V> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            ClearUsage();
            this._data.Clear();
        }

        public bool Contains(KeyValuePair<K, V> item)
        {
            return ContainsKey(item.Key);
        }

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(KeyValuePair<K, V> item)
        {
            return Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            return this._data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._data.GetEnumerator();
        }
    }
}
