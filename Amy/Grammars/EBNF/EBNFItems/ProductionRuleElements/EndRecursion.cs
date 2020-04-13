using System.Collections.Generic;

namespace Amy.Grammars.EBNF.EBNFItems.ProductionRuleElements
{
    /// <summary>
    /// EBNF EndRecursion char
    /// Is Defined like rule to allow add it to structure.
    /// Singleton
    /// </summary>
    internal class EndRecursion : IProductionRule
    {
        private static EndRecursion _current;
        public static EndRecursion Current
        {
            get 
            {
                if (_current == null)
                    _current = new EndRecursion();
                return _current;
            }
        }

        public bool IsOptional => false;

        public int MinimalLength => 1;
        public string Notation => "ε";

        /// <summary>
        /// Always true
        /// </summary>
        public bool IsExpression(string value)
        {
            return true;
        }

        /// <summary>
        /// Returns notation
        /// </summary>
        public string Rebuild()
        {
            return this.Notation;
        }

        public IEnumerable<IExpressionItem> ExpressionStructure(string value)
        {
            return null;
        }
    }
}
