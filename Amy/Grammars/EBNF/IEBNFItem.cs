using System;

namespace Amy.Grammars.EBNF
{
    /// <summary>
    /// Represents item of EBNF grammar
    /// </summary>
    internal interface IEBNFItem : IFormalGrammarItem
    {
        /// <summary>
        /// Allows to rebuilt item like is written in grammar
        /// </summary>
        /// <returns></returns>
        string Rebuild();
        /// <summary>
        /// Determines that item is optional in EBNF structure
        /// </summary>
        bool IsOptional { get; }
        /// <summary>
        /// Determines minimal length of expression for item
        /// </summary>
        int MinimalLength { get; }
        /// <summary>
        /// Determines that value is expression or not
        /// </summary>
        bool IsExpression(string value);

        /// <summary>
        /// Determines that value is expression or not
        /// </summary>
        bool IsExpression(ReadOnlyMemory<char> value);
    }
}
