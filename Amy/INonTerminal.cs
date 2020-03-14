namespace Amy
{
    public interface INonTerminal : IFormalGrammarItem, IExpressionItem
    {
        string Name { get; }
    }
}
