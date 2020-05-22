using System;
using System.Collections.Generic;

namespace Amy
{
    /// <summary>
    /// Represents every item in formal grammar
    /// </summary>
    public interface IFormalGrammarItem
    {
        /// <summary>
        /// Determines that value is item and return collection of IExpressionItems
        /// </summary>
        IEnumerable<IExpressionItem> ExpressionStructure(string value);
    }
}
