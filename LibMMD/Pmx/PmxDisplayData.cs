using System;
using System.Collections.Generic;
using System.Text;

namespace LibMMD.Pmx
{
    public class PmxDisplayData
    {
        public string NameLocal;
        public string NameUniversal;
        public bool IsSpecial;
        public int FrameCount;
        public List<FrameData> Frames;
    }

    public enum PmxDisplayFrameType : byte
    {
        Bone = 0,
        Morph = 1
    }

    public class BoneFrameData
    {
        public int BoneIndex;
    }

    public class MorphFrameData
    {
        public int MorphIndex;
    }

    public abstract class FrameData
    {
        public PmxDisplayFrameType FrameType;
    }

    public class FrameData<T> : FrameData
    {
        public T Data;
    }
}
