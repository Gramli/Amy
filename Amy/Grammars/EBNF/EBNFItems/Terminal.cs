using System;
using System.Collections.Generic;

namespace Amy.Grammars.EBNF.EBNFItems
{
    /// <summary>
    /// Represents terminal in ENBF
    /// </summary>
    internal class Terminal : IEBNFItem, ITerminal
    {
        public bool IsOptional => false;

        public string Expression { get; private set; }

        public int MinimalLength => this.Expression.Length;

        public Terminal(string value)
        {
            this.Expression = value;
        }

        /// <summary>
        /// Returns terminal in grammar
        /// </summary>
        /// <returns></returns>
        public string Rebuild()
        {
            return $"\"{this.Expression}\"";
        }

        public bool IsExpression(string value)
        {
            return this.Expression.Equals(value);
        }

        public IEnumerable<IExpressionItem> ExpressionStructure(string value)
        {
            IExpressionItem[] result = null;

            if (IsExpression(value))
            {
                var resultItem = new GrammarExpressionItem()
                {
                    Item = this,
                    Expression = value
                };

                result = new IExpressionItem[] {resultItem};
            }

            return result;
        }
    }
}
