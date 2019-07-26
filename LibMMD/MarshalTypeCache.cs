using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace LibMMD
{
    internal class MarshalTypeCache<T>
    {
        // ReSharper disable once StaticMemberInGenericType
        public static int Size { get; }

        static MarshalTypeCache()
        {
            Size = Marshal.SizeOf<T>();
        }
    }
}
