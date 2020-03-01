namespace Amy.EBNF.EBNFItems.ProductionRuleElements
{
    /// <summary>
    /// EBNF Alternation rule
    /// </summary>
    public class Alternation : IProductionRule
    {
        public const string notation = "|";

        public string Notation => Alternation.notation;

        public bool IsOptional => this._left.IsOptional && this._right.IsOptional;

        private readonly IEBNFItem _left;

        private readonly IEBNFItem _right;

        public Alternation(IEBNFItem left, IEBNFItem right)
        {
            this._left = left;
            this._right = right;
        }

        /// <summary>
        /// Resolve value. True if left item or right item
        /// </summary>
        public bool Is(string value)
        {
            return this._left.Is(value) || this._right.Is(value);
        }

        /// <summary>
        /// Rebuild Alternation rule with left and right item like is defined in grammar
        /// </summary>
        /// <returns></returns>
        public string Rebuild()
        {
            return $"{this._left.Rebuild()}{this.Notation}{this._right.Rebuild()}";
        }
    }
}
