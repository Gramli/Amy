using Amy.Caching;
using System.Collections.Generic;
using System.Text;

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

        private SmartFixedCollectionPair<string, Dictionary<string, IEBNFItem>> _cache;

        public Concatenation(IEBNFItem left, IEBNFItem right, int cacheLength)
        {
            this._left = left;
            this._right = right;
            this._cache = new SmartFixedCollectionPair<string, Dictionary<string, IEBNFItem>>(cacheLength);
            this.MinimalLength = this._left.MinimalLength + this._right.MinimalLength;
            this.IsOptional = this._left.IsOptional && this._right.IsOptional;
        }

        public bool IsExpression(string value)
        {
            if (string.IsNullOrEmpty(value) || value.Length < this.MinimalLength)
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
                var rightValue = value[(i+1)..];
                if (this._left.IsExpression(leftValue) && this._right.IsExpression(rightValue))
                {
                    Cache(value, leftValue, rightValue);
                    return true;
                }
            }

            if (this._right.IsOptional && this._left.IsExpression(value))
            {
                Cache(value, this._left);
                return true;
            }

            if (this._left.IsOptional && this._right.IsExpression(value))
            {
                Cache(value, this._right);
                return true;
            }

            return false;
        }

        private void Cache(string value, IEBNFItem item)
        {
            CacheFirstLevelSave(value, 1);
            CacheSecondLevelSave(value, value, item);
        }

        private void Cache(string value, string leftValue, string rightValue)
        {
            CacheFirstLevelSave(value, 2);
            CacheSecondLevelSave(value, leftValue, this._left);
            CacheSecondLevelSave(value, rightValue, this._right);
        }

        private void CacheSecondLevelSave(string value, string childValue, IEBNFItem item)
        {
            this._cache[value].TryAdd(childValue, item);
        }

        private void CacheFirstLevelSave(string value, int capacity)
        {
            this._cache.TryAdd(value, new Dictionary<string, IEBNFItem>(capacity));

        }

        public IEnumerable<IExpressionItem> ExpressionStructure(string value)
        {
            List<IExpressionItem> result = null;

            if (IsExpression(value))
            {
                result = new List<IExpressionItem>(25);
                foreach (var cacheValue in this._cache[value])
                {
                    var cacheValueStructure = cacheValue.Value.ExpressionStructure(cacheValue.Key);
                    if (cacheValueStructure != null)
                    {
                        result.AddRange(cacheValueStructure);
                    }
                }
            }

            return result;
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
