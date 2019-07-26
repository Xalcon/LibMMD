namespace LibMMD.DataTypes
{
    public struct Vec4f
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        public Vec4f(in float x, in float y, in float z, in float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
    }
}
