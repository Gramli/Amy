using System;
using Amy.Caching;
using System.Collections.Generic;
using System.Text;

namespace Amy.Grammars.EBNF.EBNFItems.ProductionRuleElements
{
    /// <summary>
    /// EBNF Repetition rule (group)
    /// </summary>
    internal class Repetition : IGroupProductionRule
    {
        public const string notation = "{";
        public const string endNotation = "}";
        public string Notation => Repetition.notation;
        public string EndNotation => Repetition.endNotation;

        public int MinimalLength => 0;
        public bool IsOptional => true;

        private readonly IEBNFItem _item;

        private SmartFixedCollectionPair<string, HashSet<string>> _cache;

        public Repetition(IEBNFItem item, int cacheLength)
        {
            this._item = item;
            this._cache = new SmartFixedCollectionPair<string, HashSet<string>>(cacheLength);
        }

        /// <summary>
        /// Rebuild repetition rule with item like is defined in grammar
        /// </summary>
        /// <returns></returns>
        public string Rebuild()
        {
            return $"{this.Notation}{this._item.Rebuild()}{this.EndNotation}";
        }

        public bool IsExpression(string value)
        {
            if (string.IsNullOrEmpty(value) || this._cache.ContainsKey(value))
            {
                return true;
            }

            var leftValue = string.Empty;
            for (var i = 0; i < value.Length - 1; i++)
            {
                leftValue += value[i];
                if (this._item.IsExpression(leftValue))
                {
                    var rightValue = value[(i+1)..];
                    var isRightValueExpression = IsExpression(rightValue);
                    if (isRightValueExpression)
                    {
                        Cache(value, leftValue, rightValue);
                        return true;
                    }
                }

            }

            if (this._item.IsExpression(value))
            {
                Cache(value);
                return true;
            }

            return false;
        }

        public IEnumerable<IExpressionItem> ExpressionStructure(string value)
        {
            List<IExpressionItem> result = null;

            if (IsExpression(value))
            {
                result = new List<IExpressionItem>(25);
                foreach (var cacheValue in this._cache[value])
                {
                    IEnumerable<IExpressionItem> cacheValueStructure = null;

                    if (this._cache.ContainsKey(cacheValue) && !value.Equals(cacheValue))
                    {
                        cacheValueStructure = ExpressionStructure(cacheValue);
                    }
                    else
                    {
                        cacheValueStructure = this._item.ExpressionStructure(cacheValue);
                    }

                    if (cacheValueStructure != null)
                    {
                        result.AddRange(cacheValueStructure);
                    }
                }
            }

            return result;
        }

        private void Cache(string value)
        {
            CacheFirstLevelSave(value, 1);
            CacheSecondLevelSave(value, value);
        }

        private void Cache(string value, string leftValue, string rightValue)
        {
            CacheFirstLevelSave(value, 2);
            CacheSecondLevelSave(value, leftValue);
            CacheSecondLevelSave(value, rightValue);
        }

        private void CacheSecondLevelSave(string value, string childValue)
        {
            this._cache[value].Add(childValue);
        }

        private void CacheFirstLevelSave(string value, int capacity)
        {
            this._cache.TryAdd(value, new HashSet<string>(capacity));
        }
    }
}