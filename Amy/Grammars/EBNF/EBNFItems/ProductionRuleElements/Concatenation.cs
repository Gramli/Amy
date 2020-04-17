using Amy.Caching;
using System;
using System.Collections.Generic;

namespace Amy.Grammars.EBNF.EBNFItems.ProductionRuleElements
{
    /// <summary>
    /// EBNF Concatenation rule
    /// </summary>
    internal class Concatenation : IProductionRule
    {
        public const string notation = ",";
        public string Notation => Concatenation.notation;

        public bool IsOptional { get; private set; }

        public int MinimalLength { get; private set; }

        private readonly IEBNFItem _left;

        private readonly IEBNFItem _right;

        private readonly SmartFixedCollectionPair<string, CacheItem[]> _cache;

        public Concatenation(IEBNFItem left, IEBNFItem right, int cacheLength)
        {
            this._left = left;
            this._right = right;
            this._cache = new SmartFixedCollectionPair<string, CacheItem[]>(cacheLength);
            this.MinimalLength = this._left.MinimalLength + this._right.MinimalLength;
            this.IsOptional = this._left.IsOptional && this._right.IsOptional;
        }

        public bool IsExpression(string value)
        {
            if (value.Length < this.MinimalLength)
            {
                return false;
            }

            if (this._cache.ContainsKey(value))
            {
                return true;
            }

            var leftValue = string.Empty;
            for (var i = 0; i < value.Length - 1; i++)
            {
                leftValue += value[i];
                var rightValue = value[(i + 1)..];
                if (this._left.IsExpression(leftValue) && this._right.IsExpression(rightValue))
                {
                    Cache(value, leftValue, rightValue);
                    return true;
                }
            }

            if (this._right.IsOptional && this._left.IsExpression(value))
            {
                Cache(value, true);
                return true;
            }

            if (this._left.IsOptional && this._right.IsExpression(value))
            {
                Cache(value, false);
                return true;
            }

            return false;
        }

        public bool IsExpression(ReadOnlyMemory<char> value)
        {
            return false;
        }

        private void Cache(string value, bool leftItem)
        {
            this._cache.TryAdd(value, new CacheItem[1]);
            this._cache[value][0] = new CacheItem(value, leftItem);
        }

        private void Cache(string value, string leftValue, string rightValue)
        {
            this._cache.TryAdd(value, new CacheItem[2]);
            this._cache[value][0] = new CacheItem(leftValue, true);
            this._cache[value][1] = new CacheItem(rightValue, false);
        }


        public IEnumerable<IExpressionItem> ExpressionStructure(string value)
        {
            if (IsExpression(value))
            {
                var result = new List<IExpressionItem>(25);
                foreach (var item in this._cache[value])
                {
                    var cacheValueStructure = item.Condition
                        ? this._left.ExpressionStructure(item.Value)
                        : this._right.ExpressionStructure(item.Value);

                    if (cacheValueStructure != null)
                    {
                        result.AddRange(cacheValueStructure);
                    }
                }

                return result;
            }

            return null;
        }

        /// <summary>
        /// Rebuild Concatenation rule with left and right item like is defined in grammar
        /// </summary>
        /// <returns></returns>
        public string Rebuild()
        {
            return $"{this._left.Rebuild()}{this.Notation}{this._right.Rebuild()}";
        }
    }
}
