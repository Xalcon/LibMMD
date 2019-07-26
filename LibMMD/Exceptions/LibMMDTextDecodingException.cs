using System;
using System.Collections.Generic;
using System.Text;

namespace LibMMD.Exceptions
{
    public class LibMMDTextDecodingException : LibMMDException
    {
        public LibMMDTextDecodingException(string message) : base(message)
        {
        }
    }
}
