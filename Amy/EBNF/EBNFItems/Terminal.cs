using System;
using System.Collections.Generic;

namespace Amy.EBNF.EBNFItems
{
    /// <summary>
    /// Represents terminal in ENBF
    /// </summary>
    internal class Terminal : IEBNFItem, IExpressionItem
    {
        public bool IsOptional => false;

        /// <summary>
        /// Terminal representation - its character or string
        /// </summary>
        public string Expression {get; private set;}

        public IFormalGrammarItem Item => this;

        public Terminal(string value)
        {
            this.Expression = value;
        }

        /// <summary>
        /// Resolve value using string.Equals
        /// </summary>
        public bool IsExpression(string value)
        {
            return this.Expression.Equals(value);
        }

        /// <summary>
        /// Returns terminal in grammar
        /// </summary>
        /// <returns></returns>
        public string Rebuild()
        {
            return $"\"{this.Expression}\"";
        }
    }
}
