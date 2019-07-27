using System.Runtime.InteropServices;

namespace LibMMD.DataTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Vec2f
    {
        public float X;
        public float Y;

        public Vec2f(in float x, in float y)
        {
            X = x;
            Y = y;
        }
    }
}
