using System.Runtime.InteropServices;

namespace LibMMD.DataTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Vec3f
    {
        public float X;
        public float Y;
        public float Z;

        public Vec3f(in float x, in float y, in float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
