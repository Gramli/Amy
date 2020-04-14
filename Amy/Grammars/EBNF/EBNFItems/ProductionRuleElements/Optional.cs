using Amy.Caching;
using System.Collections.Generic;

namespace Amy.Grammars.EBNF.EBNFItems.ProductionRuleElements
{
    /// <summary>
    /// EBNF Optional rule (group)
    /// </summary>
    internal class Optional : IGroupProductionRule
    {
        public const string notation = "[";

        public const string endNotation = "]";

        public string Notation => Optional.notation;
        public string EndNotation => Optional.endNotation;

        public int MinimalLength => 0;
        public bool IsOptional => true;

        private readonly IEBNFItem _item;

        private readonly SmartFixedCollection<string> _cache;


        public Optional(IEBNFItem item, int cacheLength)
        {
            this._item = item;
            this._cache = new SmartFixedCollection<string>(cacheLength);
        }

        /// <summary>
        /// Rebuild Optional rule with item like is defined in grammar
        /// </summary>
        /// <returns></returns>
        public string Rebuild()
        {
            return $"{this.Notation}{this._item.Rebuild()}{this.EndNotation}";
        }

        public bool IsExpression(string value)
        {
            if (string.IsNullOrEmpty(value) || this._cache.Contains(value))
            {
                return true;
            }

            if (value.Length >= this.MinimalLength && this._item.IsExpression(value))
            {
                this._cache.Add(value);
                return true;
            }

            return false;
        }

        public IEnumerable<IExpressionItem> ExpressionStructure(string value)
        {
            IEnumerable<IExpressionItem> result = null;

            if (IsExpression(value))
            {
                result = this._item.ExpressionStructure(value);
            }
            return result;
        }
    }
}
