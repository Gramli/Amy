using Amy.EBNF.EBNFItems;
using System.Collections.Generic;

namespace Amy.EBNF
{
    public abstract class EBNFGrammarDefinition : IFormalGrammarDefinition
    {
        public abstract string[] ProductionRules { get; }
        public abstract Dictionary<string, NonTerminal> EmptyNonTerminals { get; }
    }
}
