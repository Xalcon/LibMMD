using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using LibMMD.Exceptions;

[assembly: InternalsVisibleTo("LibMMD.Tests")]
namespace LibMMD
{
    internal static class Extensions
    {
        public static T[] Extend<T>(this T[] array, int size)
        {
            if (array.Length < size)
                Array.Resize(ref array, size);
            return array;
        }

        public static unsafe T ReadStruct<T>(this BinaryReader reader) where T : unmanaged
        {
            var buffer = reader.ReadBytes(sizeof(T));
            fixed (byte* p = buffer)
                return *(T*) p;
        }

        public static unsafe T[] ReadArray<T>(this BinaryReader reader, int elementCount) where T : unmanaged
        {
            var array = new T[elementCount];
            var buffer = reader.ReadBytes(sizeof(T) * elementCount);
            fixed (byte* src = buffer)
            {
                fixed (T* dst = array)
                {
                    Buffer.MemoryCopy(src, dst, array.Length, array.Length);
                }
            }

            return array;
        }

        public static unsafe T ReadEnum<T>(this BinaryReader reader) where T : unmanaged, Enum
        {
            var size = sizeof(T);
            switch (size)
            {
                case 1:
                    return (T)(object)reader.ReadByte();
                case 2:
                    return (T)(object)reader.ReadInt16();
                case 4:
                    return (T)(object)reader.ReadInt32();
                default:
                    throw new LibMMDParserException("Unable to read enum with size " + size);
            }
        }

        public static int ReadVarInt(this BinaryReader reader, int size, bool unsigned = false)
        {
            switch (size)
            {
                case 1:
                    return unsigned ? (int) reader.ReadByte() : reader.ReadSByte();
                case 2:
                    return unsigned ? (int) reader.ReadUInt16() : reader.ReadInt16();
                case 4:
                    return reader.ReadInt32();
                default:
                    throw new LibMMDParserException($"size of {size} not supported for variable indices");
            }
        }

        /// <summary>
        /// Reads a length prefixed string (4 bytes) from the binary reader
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ReadLPString(this BinaryReader reader, Encoding encoding)
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
