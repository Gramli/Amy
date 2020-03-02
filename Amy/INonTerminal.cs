using System;
using System.Collections.Generic;
using System.Text;

namespace Amy
{
    public interface INonTerminal : ICompiler
    {
        /// <summary>
        /// NonTerminal name
        /// </summary>
        string Name { get; }
    }
}
