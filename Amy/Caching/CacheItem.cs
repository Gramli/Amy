using System;
using System.Collections.Generic;
using System.Text;

namespace Amy.Caching
{
    internal class CacheItem
    {
        public string Value { get; }
        public bool Condition { get; }

        public CacheItem(string value, bool condition)
        {
            this.Value = value;
            this.Condition = condition;
        }

    }
}
