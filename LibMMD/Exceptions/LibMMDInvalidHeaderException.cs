using System;
using System.Collections.Generic;
using System.Text;

namespace LibMMD.Exceptions
{
    public class LibMMDInvalidHeaderException : LibMMDException
    {
        public LibMMDInvalidHeaderException(string message) : base(message)
        {
        }
    }
}
