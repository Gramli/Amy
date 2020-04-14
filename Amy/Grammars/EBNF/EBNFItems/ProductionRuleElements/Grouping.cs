using Amy.Caching;
using System.Collections.Generic;

namespace Amy.Grammars.EBNF.EBNFItems.ProductionRuleElements
{
    internal class Grouping : IGroupProductionRule
    {
        public const string notation = "(";

        public const string endNotation = ")";

        public string Notation => Grouping.notation;
        public string EndNotation => Grouping.endNotation;

        public bool IsOptional { get; private set; }

        public int MinimalLength { get; private set; }

        private readonly IEBNFItem _item;

        private readonly SmartFixedCollection<string> _cache;

        public Grouping(IEBNFItem item, int cacheLength)
        {
            this._item = item;
            this._cache = new SmartFixedCollection<string>(cacheLength);
            this.MinimalLength = this._item.MinimalLength;
            this.IsOptional = this._item.IsOptional;
        }

        /// <summary>
        /// Rebuild Grouping rule with item like is defined in grammar
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

            if(value.Length >= this.MinimalLength && this._item.IsExpression(value))
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
