using System;
using System.Collections.Generic;

namespace Amy.Grammars.EBNF
{
    /// <summary>
    /// Represents EBNF grammar
    /// </summary>
    public abstract class EBNFGrammar : IFormalGrammar
    {
        protected readonly IStartSymbol _startSymbol;

        public IStartSymbol StartSymbol => this._startSymbol;
        /// <summary>
        /// Inicialize StartSymbol
        /// </summary>
        protected EBNFGrammar(IStartSymbol startSymbol)
        {
            this._startSymbol = startSymbol;
        }

        public IEnumerable<IExpressionItem> ExpressionStructure(string value)
        {
            return this.StartSymbol.ExpressionStructure(value);
        }
    }
}
