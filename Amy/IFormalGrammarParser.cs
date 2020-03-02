using System;
using System.Collections.Generic;
using System.Text;

namespace Amy
{
    public interface IFormalGrammarParser
    {
        /// <summary>
        /// Parse grammar string. Create start symbol of grammar
        /// </summary>
        IStartSymbol Parse(IFormalGrammarDefinition definition);
    }
}
