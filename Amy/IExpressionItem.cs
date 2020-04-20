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

        //TODO DAN pouzivat nejrychlejsi kolekci - nemusi byt nutne IEnumerable
        //TODO DAN pred alokovavat
        //TODO DAN misto addrange zkusit add s for!!
        //TODO DAN Cachovat pouze v Terminalech a NonTerminalech
        IEnumerable<IExpressionItem> Childs { get; }
    }
}
