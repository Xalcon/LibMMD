using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using LibMMD.DataTypes;

namespace LibMMD.Vmd
{
    public class Vmd
    {
        public string Magic { get; internal set; }
        public string ModelName { get; internal set; }
        public VmdBoneKeyFrame[] BoneKeyFrames { get; internal set; }
        public VmdMorphKeyFrame[] MorphKeyFrames { get; internal set; }
        public VmdCameraKeyFrame[] CameraKeyFrames { get; internal set; }
        public VmdLightKeyFrame[] LightKeyFrames { get; internal set; }
        public VmdShadowKeyFrame[] ShadowKeyFrames { get; internal set; }
        public VmdInverseKinematicKeyFrame[] InverseKinematicKeyFrames { get; internal set; }

        internal Vmd()
        {
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct VmdBoneKeyFrame
    {
        private const int NAME_BYTES = 15;

        private fixed byte NameBytes[NAME_BYTES];
        public int FrameIndex;
        public Vec3f Position;
        public Vec4f RotationQuaternion;
        public fixed byte InterpolationData[64];

        public string Name
        {
            get
            {
                fixed (byte* p = this.NameBytes)
                    return Encoding.Unicode.GetString(p, NAME_BYTES).TrimEnd('\0');
            }
        }
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct VmdMorphKeyFrame
    {
        private const int NAME_BYTES = 15;
        private fixed byte NameBytes[NAME_BYTES];
        public int FrameIndex;
        public float Weight;

        public string Name
        {
            get
            {
                fixed (byte* p = this.NameBytes)
                    return Encoding.Unicode.GetString(p, NAME_BYTES).TrimEnd('\0');
            }
        }
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct VmdCameraKeyFrame
    {
        public int FrameIndex;
        public float Length;
        public Vec3f Position;
        public Vec3f Rotation;
        public fixed byte InterpolationData[24];
        public int FoVAngle;
        public byte IsPerspectiveCamera;
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct VmdLightKeyFrame
    {
        public int FrameIndex;
        public Vec3f Color;
        public Vec3f Location;
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct VmdShadowKeyFrame
    {
        public int FrameIndex;
        public byte Mode;
        public float Distance;
    }
    
    public struct VmdInverseKinematicKeyFrame
    {
        public int FrameIndex;
        public byte Show;
        public VmdInverseKinematicInfo[] IkInfos;
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct VmdInverseKinematicInfo
    {
        private const int NAME_BYTES = 20;
        private fixed byte NameBytes[NAME_BYTES];
        public byte Enabled;

        public string Name
        {
            get
            {
                fixed (byte* p = this.NameBytes)
                    return Encoding.Unicode.GetString(p, NAME_BYTES).TrimEnd('\0');
            }
        }
    }
}
