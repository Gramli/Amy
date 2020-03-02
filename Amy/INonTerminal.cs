using System;
using System.Collections.Generic;
using System.Text;

namespace Amy
{
    public interface INonTerminal
    {
        /// <summary>
        /// NonTerminal name
        /// </summary>
        string Name { get; }

        IEnumerable<INonTerminal> GetExpressionStructure(string value);
    }
}
