using System;

namespace Amy.EBNF.EBNFItems
{
    /// <summary>
    /// Represents terminal in ENBF
    /// </summary>
    internal class Terminal : IEBNFItem
    {
        /// <summary>
        /// Terminal representation - its character or string
        /// </summary>
        private readonly string _value;

        public bool IsOptional => false;

        public Terminal(string value)
        {
            this._value = value;
        }

        /// <summary>
        /// Resolve value using string.Equals
        /// </summary>
        public bool Is(string value)
        {
            return this._value.Equals(value);
        }

        /// <summary>
        /// Returns terminal in grammar
        /// </summary>
        /// <returns></returns>
        public string Rebuild()
        {
            return $"\"{this._value}\"";
        }
    }
}
