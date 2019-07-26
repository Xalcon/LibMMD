using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using LibMMD.Exceptions;

namespace LibMMD
{
    static class Extensions
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
    }
}
