using Amy.Caching;
using Amy.Extensions;
using System.Collections.Generic;
using System.Linq;

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
        /// Rebuild Alternation rule with left and right item like is defined in grammar
        /// </summary>
        /// <returns></returns>
        public string Rebuild()
        {
            return $"{this._left.Rebuild()}{this.Notation}{this._right.Rebuild()}";
        }

        public bool IsExpression(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            if (this._cache.ContainsKey(value))
            {
                return true;
            }

            if(this._left.IsExpression(value))
            {
                Cache(value, this._left);
                return true;
            }

            if(this._right.IsExpression(value))
            {
                Cache(value, this._right);
                return true;
            }

            return false;

        }

        public IEnumerable<IExpressionItem> ExpressionStructure(string value)
        {
            IEnumerable<IExpressionItem> result = null;
            if(IsExpression(value))
            {
                result = this._cache[value].ExpressionStructure(value);
            }
            return result;
        }

        private void Cache(string value, IEBNFItem item)
        {
            if (!this._cache.ContainsKey(value))
                this._cache.Add(value, item);
        }
    }
}
