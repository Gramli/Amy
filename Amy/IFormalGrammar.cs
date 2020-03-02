namespace Amy
{
    public interface IFormalGrammar : IFormalGrammarItem
    {
        /// <summary>
        /// Formal grammar start symbol
        /// </summary>
        IStartSymbol StartSymbol { get; }
    }
}
