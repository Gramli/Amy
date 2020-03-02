namespace Amy
{
    public interface IStartSymbol : INonTerminal
    {
        /// <summary>
        /// Recognize if nonterminal rule can apply on value
        /// </summary>
        bool IsNonTerminal(string nonTerminalName, string value);
        
        /// <summary>
        /// Get grammar nonterminal by name
        /// </summary>
        INonTerminal GetNonTerminal(string name);
    }
}
