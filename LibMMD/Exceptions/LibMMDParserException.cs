using System;
using System.Collections.Generic;
using System.Text;

namespace LibMMD.Exceptions
{
    public class LibMMDParserException : LibMMDException
    {
        public LibMMDParserException(string message) : base(message)
        {
        }
    }
}
