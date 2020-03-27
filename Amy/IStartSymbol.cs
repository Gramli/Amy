namespace Amy
{
    public interface IStartSymbol : INonTerminal
    {
        /// <summary>
        /// Recognize if NonTerminal rule can apply on value
        /// </summary>
        bool IsNonTerminal(string nonTerminalName, string value);

        /// <summary>
        /// Get grammar NonTerminal by name
        /// </summary>
        INonTerminal GetNonTerminal(string name);
    }
}
