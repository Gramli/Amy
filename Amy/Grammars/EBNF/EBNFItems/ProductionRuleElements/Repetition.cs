using System;
using Amy.Caching;
using System.Collections.Generic;

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

        private readonly SmartFixedCollectionPair<string, string[]> _cache;

        public Repetition(IEBNFItem item, int cacheLength)
        {
            this._item = item;
            this._cache = new SmartFixedCollectionPair<string, string[]>(cacheLength);
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
            if (this._cache.ContainsKey(value))
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
            if (IsExpression(value))
            {
                var items = this._cache[value];
                if (items.Length == 0)
                {
                    return this._item.ExpressionStructure(value);
                }

                var result = new List<IExpressionItem>(25);
                foreach (var item in items)
                {
                    if (item == null) continue;
                    result.AddRange(ExpressionStructure(item));
                }
                return result;
            }

            return null;
        }

        private void Cache(string value)
        {
            //it has to be empty, because value cannot be null
            this._cache.TryAdd(value, new string[0]);
        }

        private void Cache(string value, string leftValue, string rightValue)
        {
            this._cache.TryAdd(value, new string[2]);
            this._cache[value][0] = leftValue;
            this._cache[value][1] = rightValue;
        }
    }
}