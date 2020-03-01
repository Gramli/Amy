﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Amy.EBNF.EBNFItems
{
    /// <summary>
    /// Production Rule of EBNF
    /// </summary>
    public interface IProductionRule : IEBNFItem
    {
        /// <summary>
        /// Rule notation in EBNF
        /// </summary>
        string Notation { get; }
    }
}
