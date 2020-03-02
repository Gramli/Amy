using System;
using System.Collections.Generic;
using System.Text;

namespace Amy
{
    public interface IFormalGrammarItem
    {
        /// <summary>
        /// Determines that value is Item 
        /// </summary>
        /// <returns></returns>
        bool IsExpression(string value);
    }
}
