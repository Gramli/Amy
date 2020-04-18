using System;
using System.Collections.Generic;

namespace Amy.Grammars.EBNF.EBNFItems
{
    /// <summary>
    /// Represents terminal in ENBF
    /// </summary>
    internal class Terminal : IEBNFItem, ITerminal
    {
        public bool IsOptional => false;

        public string Expression { get; private set; }

        private readonly ReadOnlyMemory<char> _expressionMemory;

        public int MinimalLength => this.Expression.Length;

        public Terminal(string value)
        {
            this.Expression = value;
            this._expressionMemory = value.AsMemory();
        }

        /// <summary>
        /// Returns terminal in grammar
        /// </summary>
        /// <returns></returns>
        public string Rebuild()
        {
            return $"\"{this.Expression}\"";
        }

        public bool IsExpression(string value)
        {
            return this.Expression.Equals(value);
        }

        public bool IsExpression(ReadOnlyMemory<char> value)
        {
            return value.Span.Equals(this.Expression, StringComparison.Ordinal);
        }

        public IEnumerable<IExpressionItem> ExpressionStructure(string value)
        {
            IExpressionItem[] result = null;

            if (IsExpression(value))
            {
                var resultItem = new GrammarExpressionItem()
                {
                    Item = this,
                    Expression = value
                };

                result = new IExpressionItem[] {resultItem};
            }

            return result;
        }

        //public IEnumerable<IExpressionItem> ExpressionStructure(ReadOnlyMemory<char> value)
        //{
        //    IExpressionItem[] result = null;

        //    if (IsExpression(value))
        //    {
        //        var resultItem = new GrammarExpressionItem()
        //        {
        //            Item = this,
        //            Expression = value.ToString()
        //        };

        //        result = new IExpressionItem[] { resultItem };
        //    }

        //    return result;
        //}
    }
}
