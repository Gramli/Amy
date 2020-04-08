namespace Amy
{
    public interface IStartSymbol : INonTerminal
    {
        /// <summary>
        /// Get grammar NonTerminal by name
        /// </summary>
        INonTerminal GetNonTerminal(string name);
    }
}
