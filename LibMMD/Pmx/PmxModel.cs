using System.Collections.Generic;
using System.Text;
using LibMMD.Exceptions;

namespace LibMMD.Pmx
{
    public class PmxModel
    {
        public float Version { get; internal set; }
        public PmxGlobalData Globals { get; internal set; }
        public string ModelNameLocal { get; internal set; }
        public string ModelNameUniversal { get; internal set; }
        public string ModelCommentsLocal { get; internal set; }
        public string ModelCommentsUniversal { get; internal set; }
        public List<PmxVertexData> Vertices { get; internal set; }
        public List<int> Indices { get; internal set; }
        public List<string> Textures { get; internal set; }
        public List<PmxMaterial> Materials { get; internal set; }
        public List<PmxBone> Bones { get; internal set; }
        public List<PmxMorph> Morphs { get; internal set; }
        public List<PmxDisplayData> DisplayData { get; internal set; }
        public List<PmxRigidBody> RigidBodies { get; internal set; }
        public List<PmxJoint> Joints { get; internal set; }
        public List<PmxSoftBody> SoftBodies { get; internal set; }

        internal PmxModel() { }
    }

    public class PmxGlobalData
    {
        private readonly byte[] globals;

        public Encoding TextEncoding =>
            globals[0] == 0
                ? Encoding.Unicode
                : globals[0] == 1
                    ? Encoding.UTF8
                    : throw new LibMMDInvalidHeaderException($"Invalid encoding type: {globals[0]}");

        public int AdditionalVec4Count => globals[1];
        public int VertexIndexSize => globals[2];
        public int TextureIndexSize => globals[3];
        public int MaterialIndexSize => globals[4];
        public int BoneIndexSize => globals[5];
        public int MorphIndexSize => globals[6];
        public int RigidBodyIndexSize => globals[7];
        public int GlobalsCount => this.globals.Length;

        public byte this[int i] => this.globals[i];

        internal PmxGlobalData(byte[] globals)
        {
            this.globals = globals;
        }
    }
}
