using Amy.Caching;
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

        public bool IsOptional => this._left.IsOptional && this._right.IsOptional;

        private readonly IEBNFItem _left;

        private readonly IEBNFItem _right;

        private SmartFixedCollectionPair<string, Dictionary<string, IEBNFItem>> _cache;

        public Concatenation(IEBNFItem left, IEBNFItem right, int cacheLength)
        {
            this._left = left;
            this._right = right;
            this._cache = new SmartFixedCollectionPair<string, Dictionary<string, IEBNFItem>>(cacheLength);
        }

        /// <summary>
        /// Resolve value using char concat
        /// </summary>
        /// <returns></returns>
        public bool IsExpression(string value)
        {
            var result = this._cache.ContainsKey(value);
            var isNullOrEmpty = string.IsNullOrEmpty(value);
            //its much faster to check value by character than check full value, thats why full value checking is at end
            if (!isNullOrEmpty && !result)
            {
                var actualValue = string.Empty;
                for (int i = 0; i < value.Length - 1; i++)
                {
                    actualValue += value[i];
                    var ii = i + 1;
                    var restOfValue = value[ii..];
                    if (this._left.IsExpression(actualValue) && this._right.IsExpression(restOfValue))
                    {
                        result = true;
                        this._cache.Add(value, new Dictionary<string, IEBNFItem>(2));
                        this._cache[value].Add(actualValue, this._left);
                        this._cache[value].Add(restOfValue, this._right);
                        break;
                    }
                }
            }

            if (!result)
            {
                result = isNullOrEmpty && this._left.IsOptional && this._right.IsOptional;
                if (!result && this._right.IsOptional && this._left.IsExpression(value))
                {
                    result = true;
                    AddOneDictionaryItemToCache(value, this._left);
                }
                else if (!result && this._left.IsOptional && this._right.IsExpression(value))
                {
                    result = true;
                    AddOneDictionaryItemToCache(value, this._right);
                }
            }

            return result;
        }

        private void AddOneDictionaryItemToCache(string value, IEBNFItem item)
        {
            this._cache.Add(value, new Dictionary<string, IEBNFItem>(1));
            this._cache[value].Add(value, item);
        }

        public IEnumerable<IExpressionItem> ExpressionStructure(string value)
        {
            List<IExpressionItem> result = null;
            if (IsExpression(value))
            {
                result = new List<IExpressionItem>();
                foreach (var item in this._cache[value])
                {
                    result.AddRange(item.Value.ExpressionStructure(item.Key));
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
