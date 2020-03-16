using Amy.Caching;
using System.Collections.Generic;

namespace Amy.Grammars.EBNF.EBNFItems.ProductionRuleElements
{
    /// <summary>
    /// EBNF Alternation rule
    /// </summary>
    internal class Alternation : IProductionRule
    {
        public const string notation = "|";

        public string Notation => Alternation.notation;

        public bool IsOptional => this._left.IsOptional && this._right.IsOptional;

        private readonly IEBNFItem _left;

        private readonly IEBNFItem _right;

        private SmartFixedCollectionPair<string, IEBNFItem> _cache;

        public Alternation(IEBNFItem left, IEBNFItem right, int cacheLength)
        {
            this._left = left;
            this._right = right;
            this._cache = new SmartFixedCollectionPair<string, IEBNFItem>(cacheLength);
        }

        /// <summary>
        /// Resolve value. True if left item or right item
        /// </summary>
        public bool IsExpression(string value)
        {
            var result = this._cache.Contains(value);
            if (!result && this._left.IsExpression(value))
            {
                result = true;
                this._cache.Add(value, this._left);
            }
            else if (!result && this._right.IsExpression(value))
            {
                result = true;
                this._cache.Add(value, this._right);
            }
            return result;
        }

        /// <summary>
        /// Rebuild Alternation rule with left and right item like is defined in grammar
        /// </summary>
        /// <returns></returns>
        public string Rebuild()
        {
            return $"{this._left.Rebuild()}{this.Notation}{this._right.Rebuild()}";
        }

        public IEnumerable<IExpressionItem> ExpressionStructure(string value)
        {
            IEnumerable<IExpressionItem> result = null;
            if (IsExpression(value))
            {
                result = this._cache[value].ExpressionStructure(value);
            }
            return result;
        }
    }
}
