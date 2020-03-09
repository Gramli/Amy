using Amy.Cache;
using System.Collections.Generic;
using System.Text;

namespace Amy.EBNF.EBNFItems.ProductionRuleElements
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

        public bool IsOptional => true;

        private readonly IEBNFItem _item;

        private SmartFixedCollection<string> _cache;


        public Optional(IEBNFItem item, int cacheLength)
        {
            this._item = item;
            this._cache = new SmartFixedCollection<string>(cacheLength);
        }

        /// <summary>
        /// Resolve value by rule item
        /// </summary>
        public bool IsExpression(string value)
        {
            var result = this._item.IsExpression(value);
            if (result)
            {
                this._cache.Add(value);
            }
            return result;
        }

        /// <summary>
        /// Rebuild Optional rule with item like is defined in grammar
        /// </summary>
        /// <returns></returns>
        public string Rebuild()
        {
            return $"{this.Notation}{this._item.Rebuild()}{this.EndNotation}";
        }

        public IEnumerable<IExpressionItem> ExpressionStructure(string value)
        {
            IEnumerable<IExpressionItem> result = null;
            if (IsExpression(value)) result = this._item.ExpressionStructure(value);
            return result;
        }
    }
}
