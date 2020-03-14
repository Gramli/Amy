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
        /// Determines that nonterminal is definition (leftSide
        /// </summary>
        internal bool OnLeft { get; set; }
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
        public string Name { get; private set; }

        /// <summary>
        /// Implementation of IExpressionItem - actual setted expression
        /// </summary>
        public string Expression { get; private set; }
        /// <summary>
        /// Implementation of IExpressionItem
        /// </summary>
        public IFormalGrammarItem Item => this;

        private SmartFixedCollection<string> _cache;

        /// <summary>
        /// Allow to inicialize only name with set rule later
        /// </summary>
        /// <param name="name"></param>
        public NonTerminal(string name, int cacheLength)
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
            IEnumerable<IExpressionItem> result = null;
            bool isExpression = IsExpression(value);
            if (isExpression && !this.OnLeft)
            {
                this.Expression = value;
                result = new IExpressionItem[] { this };
            }
            else if (isExpression)
                result = this._rightSide.ExpressionStructure(value);
            return result;
        }
    }
}
