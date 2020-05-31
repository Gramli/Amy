using System.Collections.Generic;
using System;

namespace Amy.Caching
{
    /// <summary>
    /// Base for fixed length collection with usage count
    /// Always when item is get its usage increase
    /// </summary>
    public abstract class UsageFixedCollection<K>
    {
        private class UsageOrder
        {
            public int Usage { get; set; }
            public int OrderIndex { get; set; }
        }

        private readonly Dictionary<K, UsageOrder> _usage;
        private readonly K[] usageOrder;
        private readonly int _length;

        protected bool _full => this._usage.Count == this._length;

        protected UsageFixedCollection(int length)
        {
            this._length = length;
            this.usageOrder = new K[length];
            this._usage = new Dictionary<K, UsageOrder>(length);
        }

        protected K GetKeyToRemove()
        {
            return this.usageOrder[this._usage.Count - 1];
        }

        protected bool RemoveUsage(K key)
        {
            var usageValue = _usage[key];
            usageOrder[usageValue.OrderIndex] = default;
            return this._usage.Remove(key);

        }

        protected void AddUsage(K key)
        {
            if (_full || ContainsUsage(key))
            {
                throw new ArgumentOutOfRangeException("Can't add new item to usage collections, its full");
            }

            this._usage.Add(key, new UsageOrder());
            var orderIndex = this._usage.Count - 1;
            this._usage[key].OrderIndex = orderIndex;
            this.usageOrder[orderIndex] = key;

        }

        private void ChangeOrder(K key)
        {
            var keyOrderIndex = this._usage[key];
            if (keyOrderIndex.OrderIndex <= 0)
            {
                return;
            }
            var keyBefore = this.usageOrder[keyOrderIndex.OrderIndex - 1];
            var keyBeforeOrderIndex = this._usage[keyBefore];
            if (keyBeforeOrderIndex.Usage >= keyOrderIndex.Usage)
            {
                return;
            }
            var temp = this.usageOrder[keyOrderIndex.OrderIndex];
            this.usageOrder[keyOrderIndex.OrderIndex] = keyBefore;
            this.usageOrder[keyBeforeOrderIndex.OrderIndex] = temp;
            keyOrderIndex.OrderIndex--;
            keyBeforeOrderIndex.OrderIndex++;
        }

        protected void IncreaseUsage(K key)
        {
            this._usage[key].Usage++;
            ChangeOrder(key);
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
