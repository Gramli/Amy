namespace Amy.EBNF
{
    /// <summary>
    /// EBNF Grammar parser interface
    /// </summary>
    public interface IEBNFGrammarParser
    {
        /// <summary>
        /// Parse grammar string. Create start symbol of grammar
        /// </summary>
        IEBNFStartSymbol Parse(string grammar);
    }
}
