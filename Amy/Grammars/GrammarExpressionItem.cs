using System.Collections.Generic;

namespace Amy.Grammars
{
    public class GrammarExpressionItem : IExpressionItem
    {
        public string Expression { get; set; }

        public IFormalGrammarItem Item { get; set; }

        public IEnumerable<IExpressionItem> Childs { get; set; }
    }
}
