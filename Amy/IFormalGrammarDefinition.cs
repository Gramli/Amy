using System;
using System.Collections.Generic;
using System.Text;

namespace Amy
{
    public interface IFormalGrammarDefinition
    {
        string[] ProductionRules { get; }
    }
}
