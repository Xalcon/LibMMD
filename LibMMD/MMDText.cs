using LibMMD.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("LibMMD.Tests")]
namespace LibMMD
{
    internal static class MMDText
    {
        public static string Read(BinaryReader reader, Encoding encoding)
        {
            if (reader.BaseStream.Length - reader.BaseStream.Position < 4)
                throw new LibMMDTextDecodingException("Unable to decode text from undersized buffer");

            var len = reader.ReadInt32();
            var remainingBytes = (reader.BaseStream.Length - reader.BaseStream.Position);
            if (len < 0 || len > remainingBytes)
                throw new LibMMDTextDecodingException($"Text to big for array buffer (Len: {len}, Remaining Bytes: {remainingBytes})");

            var stringBytes = reader.ReadBytes(len);
            return len > 0 ? encoding.GetString(stringBytes) : "";
        }
    }
}
