namespace Amy.EBNF.EBNFItems
{
    /// <summary>
    /// Production Rule of EBNF
    /// </summary>
    internal interface IProductionRule : IEBNFItem
    {
        /// <summary>
        /// Rule notation in EBNF
        /// </summary>
        string Notation { get; }
    }
}
