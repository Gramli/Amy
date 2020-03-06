using System.Collections.Generic;
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

        private List<IEBNFItem> _lastExpressionItems;

        public Concatenation(IEBNFItem left, IEBNFItem right)
        {
            this._left = left;
            this._right = right;
            this._lastExpressionItems = new List<IEBNFItem>();
        }

        /// <summary>
        /// Resolve value using char concat
        /// </summary>
        /// <returns></returns>
        public bool IsExpression(string value)
        {
            var result = false;
            var isNullOrEmpty = string.IsNullOrEmpty(value);
            this._lastExpressionItems.Clear();
            //its much faster to check value by character than check full value, thats why full value checking is at end

            if (!isNullOrEmpty)
            {
                var actualValue = string.Empty;
                for (int i = 0; i < value.Length - 1; i++)
                {
                    actualValue += value[i];
                    var ii = i + 1;
                    var restOfValue = value.Substring(ii, value.Length - ii);
                    if (this._left.IsExpression(actualValue) && this._right.IsExpression(restOfValue))
                    {
                        result = true;
                        this._lastExpressionItems.Add(this._left);
                        this._lastExpressionItems.Add(this._right);
                        break;
                    }
                }
            }

            if (!result)
            {
                if (isNullOrEmpty && this._left.IsOptional && this._right.IsOptional)
                    result = true;
                else if (this._right.IsOptional && this._left.IsExpression(value))
                {
                    result = true;
                    this._lastExpressionItems.Add(this._left);
                }
                else if (this._left.IsOptional && this._right.IsExpression(value))
                {
                    result = true;
                    this._lastExpressionItems.Add(this._right);
                }
            }

            return result;
        }

        public IEnumerable<IFormalGrammarItem> FetchLastExpressionStructure()
        {
            List<IFormalGrammarItem> result = null;
            foreach (var item in this._lastExpressionItems)
                if (item is IProductionRule)
                    result.AddRange(item.CheckExpressionAndFetchStructure());
                else
                    result.Add(item);
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
