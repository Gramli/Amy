﻿namespace Amy.EBNF
{
    /// <summary>
    /// Represents item of EBNF grammar
    /// </summary>
    internal interface IEBNFItem
    {
        /// <summary>
        /// Determines that value is IEBNFItem 
        /// </summary>
        /// <returns></returns>
        bool Is(string value);
        /// <summary>
        /// Allows to rebuilt item like is written in grammar
        /// </summary>
        /// <returns></returns>
        string Rebuild();

        /// <summary>
        /// Determines that item is optional in EBNF structure
        /// </summary>
        bool IsOptional { get; }
    }
}
