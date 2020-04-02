using System.Collections;
using System.Collections.Generic;

namespace Amy.Caching
{
    public class SmartFixedCollection<K> : UsageFixedCollection<K>, ICollection<K> where K : class
    {
        int ICollection<K>.Count => Count();

        public bool IsReadOnly => true;

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

        public override bool Remove(K key)
        {
            RemoveUsage(key);
            return true;
        }

        public override bool Contains(K key)
        {
            return ContainsUsage(key);
        }

        public IEnumerator<K> GetEnumerator()
        {
            return GetUsageEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetUsageEnumerator();
        }

        public void Clear()
        {
            ClearUsage();
        }

        public void CopyTo(K[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }
    }
}
