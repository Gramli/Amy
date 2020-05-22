using Amy.Grammars.EBNF.EBNFItems;
using System.Collections.Generic;

namespace Amy.Grammars.EBNF
{
    /// <summary>
    /// Base class for EBNF grammar definition
    /// </summary>
    public abstract class EBNFGrammarDefinition : IFormalGrammarDefinition
    {
        public abstract string[] ProductionRules { get; }
        public abstract NonTerminal GetNewNonTerminalInstance(string name);

        public abstract EBNFStartSymbol GetStartSymbol(NonTerminal startSymbolNonTerminal, List<NonTerminal> rules);
    }
}
