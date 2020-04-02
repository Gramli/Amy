using Amy.Caching;
using Amy.Exceptions;
using System;
using System.Collections.Generic;

namespace Amy.Grammars.EBNF.EBNFItems
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

        public IFormalGrammarItem Rule => this._rightSide;
        /// <summary>
        /// 
        /// </summary>
        public bool IsOptional => this._rightSide.IsOptional;
        /// <summary>
        /// NonTerminal Name, left side of definition
        /// </summary>
        public string Name { get; private set; }

        private readonly SmartFixedCollection<string> _cache;

        /// <summary>
        /// Allow to inicialize only name with set rule later
        /// </summary>
        /// <param name="name"></param>
        protected NonTerminal(string name, int cacheLength)
        {
            this.Name = name;
            this._cache = new SmartFixedCollection<string>(cacheLength);
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
            return $"{this.Name}";
        }

        /// <summary>
        /// Resolve value using right side rule
        /// </summary>
        public bool IsExpression(string value)
        {
            if (this._rightSide == null)
                throw new GrammarParseException($"Right side rule of NonTerminal: {this.Name} is null.", new NullReferenceException());
            var result = this._rightSide.IsExpression(value) || this._cache.Contains(value);

            if (result && !this._cache.Contains(value))
            {
                this._cache.Add(value);
            }
            return result;
        }

        public IEnumerable<IExpressionItem> ExpressionStructure(string value)
        {
            IExpressionItem[] result = null;

            var isExpression = IsExpression(value);
            if (isExpression)
            {
                var resultItem = new GrammarExpressionItem()
                {
                    Item = this,
                    Expression = value,
                    Childs = this._rightSide.ExpressionStructure(value)
                };

                result = new IExpressionItem[] { resultItem };
            }

            return result;
        }
    }
}
