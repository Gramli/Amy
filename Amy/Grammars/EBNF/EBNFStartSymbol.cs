using Amy.Exceptions;
using Amy.Grammars.EBNF.EBNFItems;
using System.Collections.Generic;

namespace Amy.Grammars.EBNF
{
    /// <summary>
    /// Implementation of EBNF start symbol
    /// </summary>
    public abstract class EBNFStartSymbol : IStartSymbol
    {
        /// <summary>
        /// Production rules cached by its names
        /// </summary>
        private readonly Dictionary<string, NonTerminal> _productionRules;

        /// <summary>
        /// Start symbol rule
        /// </summary>
        private readonly NonTerminal _startSymbolNonTerminal;

        public string Expression => this._startSymbolNonTerminal.Expression;

        public string Name => this._startSymbolNonTerminal.Name;

        public IFormalGrammarItem Item => this._startSymbolNonTerminal.Item;

        public EBNFStartSymbol(NonTerminal startSymbolNonTerminal, IEnumerable<NonTerminal> productionRules)
        {
            this._startSymbolNonTerminal = startSymbolNonTerminal;
            this._productionRules = new Dictionary<string, NonTerminal>();
            InicializeProductionRules(productionRules);
        }

        private void InicializeProductionRules(IEnumerable<NonTerminal> productionRules)
        {
            foreach (NonTerminal productionRule in productionRules)
                this._productionRules[productionRule.Name] = productionRule;
        }

        /// <summary>
        /// Determines that value is grammar start symbol expression
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsExpression(string value)
        {
            return this._startSymbolNonTerminal.IsExpression(value);
        }

        /// <summary>
        /// Check value by nonTerminal rule. NonTerminal is selected by name
        /// </summary>
        public bool IsNonTerminal(string nonTerminalName, string value)
        {
            return this._productionRules[nonTerminalName].IsExpression(value);
        }

        /// <summary>
        /// Retunrs nonTerminal by name
        /// </summary>
        INonTerminal IStartSymbol.GetNonTerminal(string name)
        {
            if (!this._productionRules.ContainsKey(name))
                throw new MissingNonTerminalException("There is missing non terminal.", new KeyNotFoundException());
            return this._productionRules[name];
        }

        public IEnumerable<IExpressionItem> ExpressionStructure(string value)
        {
            return this._startSymbolNonTerminal.ExpressionStructure(value);
        }
    }
}
