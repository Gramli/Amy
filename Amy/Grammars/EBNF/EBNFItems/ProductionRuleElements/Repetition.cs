using Amy.Caching;
using Amy.Extensions;
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
            if (string.IsNullOrEmpty(value))
            {
                return true;
            }

            if (this._cache.ContainsKey(value))
            {
                return true;
            }

            var builder = new StringBuilder();
            for (var i = 0; i < value.Length - 1; i++)
            {
                var leftValue = builder.Append(value[i]).ToString();
                if(this._item.IsExpression(leftValue))
                {
                    var ii = i + 1;
                    var rightValue = value[ii..];
                    var isRightValueExpression = IsExpression(rightValue);
                    if(isRightValueExpression)
                    {
                        Cache(value, leftValue, rightValue);
                        return true;
                    }

                }

            }

            if(this._item.IsExpression(value))
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
                result = new List<IExpressionItem>();
                foreach (var cacheValue in this._cache[value])
                {
                    IEnumerable<IExpressionItem> cacheValueStructure = null;

                    if (this._cache.ContainsKey(cacheValue) && !value.Equals(cacheValue))
                    {
                        cacheValueStructure = ExpressionStructure(cacheValue);
                    }
                    else
                    {
                        cacheValueStructure = this._item.ExpressionStructure(value);
                    }

                    if (cacheValueStructure != null)
                    {
                        result.AddRange(cacheValueStructure);
                    }
                }
            }

            return result;
        }

        private IEnumerable<IExpressionItem> ExpressionStructure1(string value)
        {
            List<IExpressionItem> result = new List<IExpressionItem>();
            foreach (var cacheValue in this._cache[value])
            {
                var cacheValueStructure = ExpressionStructure1(cacheValue);
                if (cacheValueStructure != null)
                {
                    result.AddRange(cacheValueStructure);
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
            if (!this._cache[value].Contains(childValue))
            {
                this._cache[value].Add(childValue);
            }
        }

        private void CacheFirstLevelSave(string value, int capactity)
        {
            if (!this._cache.ContainsKey(value))
            {
                this._cache.Add(value, new HashSet<string>());
            }
        }
    }
}