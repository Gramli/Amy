using System;
using System.Collections.Generic;
using System.Text;

namespace Amy
{
    /// <summary>
    /// Encapsulates string representation of expression with item
    /// </summary>
    public interface IExpressionItem
    {
        string Expression { get; }

        IFormalGrammarItem Item { get; }
    }
}
