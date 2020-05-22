using System.Collections.Generic;

namespace Amy.Grammars.EBNF.EBNFItems.ProductionRuleElements
{
    /// <summary>
    /// Implementation of EBNF Alternation rule
    /// </summary>
    internal class Alternation : IProductionRule
    {
        public const string notation = "|";

        public string Notation => Alternation.notation;

        public bool IsOptional { get; private set; }

        public int MinimalLength { get; private set; }

        private readonly IEBNFItem _left;

        private readonly IEBNFItem _right;

        public Alternation(IEBNFItem left, IEBNFItem right)
        {
            this._left = left;
            this._right = right;
            this.MinimalLength = this._left.MinimalLength <= this._right.MinimalLength ? this._left.MinimalLength : this._right.MinimalLength;
            this.IsOptional = this._left.IsOptional && this._right.IsOptional;
        }

        /// <summary>
        /// Rebuild Alternation rule with left and right item like is defined in grammar
        /// </summary>
        public string Rebuild()
        {
            return $"{this._left.Rebuild()}{this.Notation}{this._right.Rebuild()}";
        }

        public bool IsExpression(string value)
        {
            return CheckMinimalLength(value.Length) && (IsLeft(value) || IsRight(value));
        }

        protected bool CheckMinimalLength(int valueLength)
        {
            return valueLength >= this.MinimalLength;
        }

        protected bool IsLeft(string value)
        {
            return this._left.IsExpression(value);
        }

        protected bool IsRight(string value)
        {
            return this._right.IsExpression(value);
        }

        public IEnumerable<IExpressionItem> ExpressionStructure(string value)
        {
            if (!CheckMinimalLength(value.Length))
            {
                return null;
            }
            if (IsLeft(value))
            {
                return this._left.ExpressionStructure(value);
            }
            if (IsRight(value))
            {
                return this._right.ExpressionStructure(value);
            }
            return null;
        }
    }
}
