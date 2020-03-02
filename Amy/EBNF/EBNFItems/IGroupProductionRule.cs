namespace Amy.EBNF.EBNFItems
{
    /// <summary>
    /// Group production rule in EBNF
    /// </summary>
    internal interface IGroupProductionRule : IProductionRule
    {
        /// <summary>
        /// End notation of production rule in EBNF
        /// </summary>
        string EndNotation { get; }
    }
}
