namespace Amy
{
    public interface INonTerminal : IFormalGrammarItem
    {
        string Name { get; }

        IFormalGrammarItem Rule { get; }
    }
}
