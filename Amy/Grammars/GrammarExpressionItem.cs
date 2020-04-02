using System;
using System.Collections.Generic;
using System.Text;

namespace Amy.Grammars
{
    public class GrammarExpressionItem : IExpressionItem
    {
        public string Expression { get; set; }

        public IFormalGrammarItem Item { get; set; }

        public IEnumerable<IExpressionItem> Childs { get; set; }
    }
}
