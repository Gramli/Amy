using System;
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

        public Grouping(IEBNFItem item)
        {
            this._item = item;
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
            return this._item.IsExpression(value);
        }

        public IEnumerable<IExpressionItem> ExpressionStructure(string value)
        {
            if (IsExpression(value))
            {
                return this._item.ExpressionStructure(value);
            }
            return null;
        }

    }
}
