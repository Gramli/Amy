namespace Amy
{
    public interface IFormalGrammar : ICompiler
    {
        /// <summary>
        /// Formal grammar start symbol
        /// </summary>
        IStartSymbol StartSymbol { get; }
        /// <summary>
        /// Determines that value is grammar start symbol expression
        /// </summary>
        bool IsExpression(string value);
    }
}
