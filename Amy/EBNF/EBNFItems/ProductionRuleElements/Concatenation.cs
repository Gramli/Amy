using System.Text;

namespace Amy.EBNF.EBNFItems.ProductionRuleElements
{
    /// <summary>
    /// EBNF Concatenation rule
    /// </summary>
    internal class Concatenation : IProductionRule
    {
        public const string notation = ",";
        public string Notation => Concatenation.notation;

        public bool IsOptional => this._left.IsOptional && this._right.IsOptional;

        private readonly IEBNFItem _left;

        private readonly IEBNFItem _right;

        public Concatenation(IEBNFItem left, IEBNFItem right)
        {
            this._left = left;
            this._right = right;
        }

        /// <summary>
        /// Resolve value using char concat
        /// </summary>
        /// <returns></returns>
        public bool Is(string value)
        {
            var result = false;
            var isNullOrEmpty = string.IsNullOrEmpty(value);

            //its much faster to check value by character than check full value, thats why full value checking is at end

            if (!isNullOrEmpty)
            {
                var actualValue = string.Empty;
                for (int i = 0; i < value.Length - 1; i++)
                {
                    actualValue += value[i];
                    var ii = i + 1;
                    var restOfValue = value.Substring(ii, value.Length - ii);
                    if (this._left.Is(actualValue) && this._right.Is(restOfValue))
                    {
                        result = true;
                        break;
                    }
                }
            }

            if(!result)
            {
                result = isNullOrEmpty && this._left.IsOptional && this._right.IsOptional ||
                    this._right.IsOptional && this._left.Is(value) || this._left.IsOptional && this._right.Is(value);
            }

            return result;
        }

        /// <summary>
        /// Rebuild Concatenation rule with left and right item like is defined in grammar
        /// </summary>
        /// <returns></returns>
        public string Rebuild()
        {
            return $"{this._left.Rebuild()}{this.Notation}{this._right.Rebuild()}";
        }
    }
}
