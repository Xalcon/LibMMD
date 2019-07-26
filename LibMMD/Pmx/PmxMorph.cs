using System;
using System.Collections.Generic;
using System.Text;
using LibMMD.DataTypes;

namespace LibMMD.Pmx
{
    public abstract class PmxMorph
    {
        public int Index;
        public string NameLocal;
        public string NameUniversal;
        public byte PanelType;
        public PmxMorphType MorphType;
        public int Count;
    }

    public class PmxMorph<T> : PmxMorph
    {
        public List<T> Data;
    }

    public struct GroupMorph
    {
        public int MorphIndex;
        public float Influence;
    }

    public struct VertexMorph
    {
        public int VertexIndex;
        public Vec3f Translation;
    }

    public struct BoneMorph
    {
        public int BoneIndex;
        public Vec3f Translation;
        public Vec4f Rotation;
    }

    public struct UVMorph
    {
        public int VertexIndex;
        public Vec4f Changes;
    }

    public struct MaterialMorph
    {
        public int MaterialIndex;
        public byte Unkn;
        public Vec4f DiffuseColor;
        public Vec3f SpecularColor;
        public float Specularity;
        public Vec3f AmbientColor;
        public Vec4f EdgeColor;
        public float EdgeSize;
        public Vec4f TextureTint;
        public Vec4f EnvironmentTint;
        public Vec4f ToonTint;
    }

    public struct FlipMorph
    {
        public int MorphIndex;
        public float Influence;
    }

    public struct ImpulseMorph
    {
        public int RigidBodyIndex;
        public byte LocalFlag;
        public Vec3f MovementSpeed;
        public Vec3f RotationTorque;
    }

    public enum PmxMorphType: byte
    {
        Group,
        Vertex,
        Bone,
        UV,
        UVExt1,
        UVExt2,
        UVExt3,
        UVExt4,
        Material,
        Flip,
        Impulse
    }
}
