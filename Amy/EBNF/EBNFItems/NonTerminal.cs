﻿using Amy.Cache;
using System;
using System.Collections.Generic;

namespace Amy.EBNF.EBNFItems
{
    /// <summary>
    /// Represents NonTerminal in EBNF
    /// </summary>
    public abstract class NonTerminal : IEBNFItem, INonTerminal
    {
        public const string Definition = "=";
        /// <summary>
        /// NonTerminal value on right side
        /// </summary>
        private IEBNFItem _rightSide;
        /// <summary>
        /// 
        /// </summary>
        public bool IsOptional => this._rightSide.IsOptional;
        /// <summary>
        /// NonTerminal Name, left side of definition
        /// </summary>
        public string Expression { get; private set; }

        public IFormalGrammarItem Item => this;

        private SmartFixedCollection<string> _cache;

        /// <summary>
        /// Allow to inicialize only name with set rule later
        /// </summary>
        /// <param name="name"></param>
        public NonTerminal(string name)
        {
            this.Expression = name;
            this._cache = new SmartFixedCollection<string>(20);
        }

        /// <summary>
        /// Allows to set right side rule
        /// </summary>
        internal void SetRightSide(IEBNFItem item)
        {
            this._rightSide = item;
        }

        /// <summary>
        /// Returns NonTerminal name
        /// </summary>
        public string Rebuild()
        {
            return $"{this.Expression}";
        }

        /// <summary>
        /// Resolve value using right side rule
        /// </summary>
        public bool IsExpression(string value)
        {
            if (this._rightSide == null)
                throw new NullReferenceException($"Right side rule of NonTerminal: {this.Expression} is null.");
            var result = this._rightSide.IsExpression(value) || this._cache.Contains(value);

            if (result && !this._cache.Contains(value))
            {
                this._cache.Add(value);
            }
            return result;
        }
    }
}
