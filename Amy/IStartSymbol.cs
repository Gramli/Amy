namespace Amy
{
    public interface IStartSymbol : ICompiler
    {
        /// <summary>
        /// Recognize if nonterminal rule can apply on value
        /// </summary>
        bool IsNonTerminal(string nonTerminalName, string value);

        /// <summary>
        /// Determines that value is grammar start symbol expression
        /// </summary>
        bool IsExpression(string value);

        /// <summary>
        /// Get grammar nonterminal by name
        /// </summary>
        INonTerminal GetNonTerminal(string name);
    }
}
