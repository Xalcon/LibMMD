using System;
using System.Collections.Generic;
using System.Text;

namespace LibMMD.Exceptions
{
    public class LibMMDUnsupportedEncodingException : LibMMDException
    {
        public LibMMDUnsupportedEncodingException(string message) : base(message)
        {
        }
    }
}
