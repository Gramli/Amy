using System.Text;

namespace Amy.EBNF.EBNFItems.ProductionRuleElements
{
    /// <summary>
    /// EBNF Optional rule (group)
    /// </summary>
    public class Optional : IGroupProductionRule
    {
        public const string notation = "[";

        public const string endNotation = "]";

        public string Notation => Optional.notation;
        public string EndNotation => Optional.endNotation;

        public bool IsOptional => true;

        private readonly IEBNFItem _item;

        public Optional(IEBNFItem item)
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
        /// Rebuild Optional rule with item like is defined in grammar
        /// </summary>
        /// <returns></returns>
        public string Rebuild()
        {
            return $"{this.Notation}{this._item.Rebuild()}{this.EndNotation}";
        }
    }
}
