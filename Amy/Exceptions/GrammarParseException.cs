using System;
using System.Collections.Generic;
using System.Text;

namespace Amy.Exceptions
{
    public class GrammarParseException : Exception
    {
        public GrammarParseException(string message)
            : base(message)
        {

        }

        public GrammarParseException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
