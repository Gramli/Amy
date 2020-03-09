using Amy.Cache;
using System.Collections.Generic;

namespace Amy.EBNF.EBNFItems.ProductionRuleElements
{
    internal class Grouping : IGroupProductionRule
    {
        public const string notation = "(";

        public const string endNotation = ")";

        public string Notation => Grouping.notation;
        public string EndNotation => Grouping.endNotation;

        public bool IsOptional => _item.IsOptional;

        private readonly IEBNFItem _item;

        private SmartFixedCollection<string> _cache;

        public Grouping(IEBNFItem item, int cacheLength)
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
            if(result)
            {
                this._cache.Add(value);
            }
            return result;
        }

        /// <summary>
        /// Rebuild Grouping rule with item like is defined in grammar
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
