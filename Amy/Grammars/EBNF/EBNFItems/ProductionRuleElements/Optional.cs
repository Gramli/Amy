using System;
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


        public Optional(IEBNFItem item)
        {
            this._item = item;
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
            return this._item.IsExpression(value);
        }

        public bool IsExpression(ReadOnlyMemory<char> value)
        {
            return false;
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
