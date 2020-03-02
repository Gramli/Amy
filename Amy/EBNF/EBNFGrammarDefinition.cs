using Amy.EBNF.EBNFItems;
using System;
using System.Collections.Generic;

namespace Amy.EBNF
{
    public class EBNFGrammarDefinition : IFormalGrammarDefinition
    {
        public string[] ProductionRules => throw new NotImplementedException();
        public Dictionary<string, NonTerminal> EmptyNonTerminals { get; }
    }
}
