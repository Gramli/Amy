using System;

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
        /// NonTerminal Name, left side of definition
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsOptional => this._rightSide.IsOptional;

        /// <summary>
        /// Allow to inicialize only name with set rule later
        /// </summary>
        /// <param name="name"></param>
        public NonTerminal(string name)
        {
            this.Name = name;
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
        public bool Is(string value)
        {
            if (this._rightSide == null)
                throw new NullReferenceException($"Right side rule of NonTerminal: {this.Name} is null.");
            return this._rightSide.Is(value);
        }
    }
}
