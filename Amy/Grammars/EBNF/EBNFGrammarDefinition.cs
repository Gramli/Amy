using Amy.Grammars.EBNF.EBNFItems;
using System.Collections.Generic;

namespace Amy.Grammars.EBNF
{
    public abstract class EBNFGrammarDefinition : IFormalGrammarDefinition
    {
        public abstract string[] ProductionRules { get; }
        public abstract NonTerminal GetNewNonTerminalInstance(string name);
    }
}
