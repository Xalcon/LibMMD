using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace LibMMD.Exceptions
{
    public abstract class LibMMDException : Exception
    {
        public LibMMDException()
        {
        }

        public LibMMDException(string message) : base(message)
        {
        }

        public LibMMDException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
