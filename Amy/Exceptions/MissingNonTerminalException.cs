using System;
using System.Collections.Generic;
using System.Text;

namespace Amy.Exceptions
{
    public class MissingNonTerminalException : Exception
    {
        public MissingNonTerminalException(string message)
            : base(message)
        {

        }

        public MissingNonTerminalException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
