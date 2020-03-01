namespace Amy.EBNF.EBNFItems.ProductionRuleElements
{
    public class Grouping : IGroupProductionRule
    {
        public const string notation = "(";

        public const string endNotation = ")";

        public string Notation => Grouping.notation;
        public string EndNotation => Grouping.endNotation;

        public bool IsOptional => _item.IsOptional;

        private readonly IEBNFItem _item;

        public Grouping(IEBNFItem item)
        {
            this._item = item;
        }

        /// <summary>
        /// Resolve value by rule item
        /// </summary>
        public bool Is(string value)
        {
            return this._item.Is(value);
        }

        /// <summary>
        /// Rebuild Grouping rule with item like is defined in grammar
        /// </summary>
        /// <returns></returns>
        public string Rebuild()
        {
            return $"{this.Notation}{this._item.Rebuild()}{this.EndNotation}";
        }
    }
}
