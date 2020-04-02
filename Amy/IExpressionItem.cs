using System.Collections.Generic;

namespace Amy
{
    /// <summary>
    /// Encapsulates string representation of expression with item
    /// </summary>
    public interface IExpressionItem
    {
        string Expression { get; }

        IFormalGrammarItem Item { get; }

        IEnumerable<IExpressionItem> Childs { get; }
    }
}
