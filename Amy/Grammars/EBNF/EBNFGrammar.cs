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

        public INonTerminal GetNonTerminal(string name)
        {
            return this.StartSymbol.GetNonTerminal(name);
        }


        /// <summary>
        /// Determines that value is grammar start symbol expression
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsExpression(string value)
        {
            return this.StartSymbol.IsExpression(value);
        }

        /// <summary>
        /// Check if value is nonTerminal defined by name
        /// </summary>
        public bool IsNonTerminal(string nonTerminalName, string value)
        {
            return this.StartSymbol.IsNonTerminal(nonTerminalName, value);
        }

        public IEnumerable<IExpressionItem> ExpressionStructure(string value)
        {
            return this.StartSymbol.ExpressionStructure(value);
        }
    }
}
