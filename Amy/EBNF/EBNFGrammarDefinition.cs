using Amy.EBNF.EBNFItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amy.EBNF
{
    public class EBNFGrammarDefinition : IFormalGrammarDefinition
    {
        public string[] ProductionRules => throw new NotImplementedException();
        public Dictionary<string, NonTerminal> EmptyNonTerminals { get; }
    }
}
