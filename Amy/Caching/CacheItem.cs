using System;
using System.Collections.Generic;
using System.Text;

namespace Amy.Caching
{
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
