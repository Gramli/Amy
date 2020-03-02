using System.Text;

namespace Amy.EBNF.EBNFItems.ProductionRuleElements
{
    /// <summary>
    /// EBNF Repetition rule (group)
    /// </summary>
    internal class Repetition : IGroupProductionRule
    {
        public const string notation = "{";
        public const string endNotation = "}";
        public string Notation => Repetition.notation;
        public string EndNotation => Repetition.endNotation;

        public bool IsOptional => true;

        private readonly IEBNFItem _item;

        public Repetition(IEBNFItem item)
        {
            this._item = item;
        }

        /// <summary>
        /// Resolve value using by chars concat
        /// </summary>
        /// <returns></returns>
        public bool Is(string value)
        {
            var result = string.IsNullOrEmpty(value);
            if (!result)
            {
                var builder = new StringBuilder();
                for (var i = 0; i < value.Length -1; i++)
                {
                    builder.Append(value[i]);
                    if (this._item.Is(builder.ToString()))
                    {
                        var ii = i + 1;
                        var restOfValue = value.Substring(ii, value.Length - ii);
                        result = Is(restOfValue);
                        break;
                    }
                }
            }
            if(!result)
            {
                result = this._item.Is(value);
            }
            return result;
        }

        /// <summary>
        /// Rebuild repetition rule with item like is defined in grammar
        /// </summary>
        /// <returns></returns>
        public string Rebuild()
        {
            return $"{this.Notation}{this._item.Rebuild()}{this.EndNotation}";
        }
    }
}