using System.Collections.Generic;

namespace Amy
{
    public interface IFormalGrammarItem
    {
        /// <summary>
        /// Determines that value is Item 
        /// </summary>
        bool IsExpression(string value);

        /// <summary>
        /// Determines that value is item and return collection of IExpressionItems
        /// </summary>
        IEnumerable<IExpressionItem> ExpressionStructure(string value);
    }
}
