using System.Collections.Generic;

namespace Amy
{
    public interface IFormalGrammarItem
    {
        /// <summary>
        /// Determines that value is Item 
        /// </summary>
        bool IsExpression(string value);
    }
}
