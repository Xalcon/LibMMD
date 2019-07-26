using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LibMMD.DataTypes;

namespace LibMMD.Pmx
{
    public struct PmxVertexData
    {
        public Vec3f Position;
        public Vec3f Normal;
        public Vec2f UV;
        public Vec4f[] AdditionalVec4;
        public BoneWeightDeformType DeformType;
        public IBoneDeformData DeformData;
        public float EdgeScale;
    }

    public enum BoneWeightDeformType : byte
    {
        BDEF1,
        BDEF2,
        BDEF4,
        SDEF,
        QDEF
    }

    public interface IBoneDeformData
    {
        BDEF1 AsBDEF1();
        BDEF2 AsBDEF2();
        BDEF4 AsBDEF4();
        SDEF AsSDEF();
        QDEF AsQDEF();
    }

    public struct BDEF1 : IBoneDeformData
    {
        /// <summary>
        /// weight == 1
        /// </summary>
        public int BoneIndex;

        public BDEF1 AsBDEF1()
        {
            return this;
        }

        public BDEF2 AsBDEF2()
        {
            throw new NotImplementedException();
        }

        public BDEF4 AsBDEF4()
        {
            throw new NotImplementedException();
        }

        public SDEF AsSDEF()
        {
            throw new NotImplementedException();
        }

        public QDEF AsQDEF()
        {
            throw new NotImplementedException();
        }

        public static BDEF1 Read(BinaryReader reader, PmxGlobalData globalData)
        {
            return new BDEF1()
            {
                BoneIndex = reader.ReadVarInt(globalData.BoneIndexSize)
            };
        }
    }

    public struct BDEF2 : IBoneDeformData
    {
        public int BoneIndex1;
        public int BoneIndex2;
        /// <summary>
        /// Bone 2 weight = 1.0 - Bone 1 weight
        /// </summary>
        public float Bone1Weight;

        public BDEF1 AsBDEF1()
        {
            throw new NotImplementedException();
        }

        public BDEF2 AsBDEF2()
        {
            return this;
        }

        public BDEF4 AsBDEF4()
        {
            throw new NotImplementedException();
        }

        public SDEF AsSDEF()
        {
            throw new NotImplementedException();
        }

        public QDEF AsQDEF()
        {
            throw new NotImplementedException();
        }

        public static BDEF2 Read(BinaryReader reader, PmxGlobalData globalData)
        {
            return new BDEF2()
            {
                BoneIndex1 = reader.ReadVarInt(globalData.BoneIndexSize),
                BoneIndex2 = reader.ReadVarInt(globalData.BoneIndexSize),
                Bone1Weight = reader.ReadSingle(),
            };
        }
    }

    public struct BDEF4 : IBoneDeformData
    {
        public int BoneIndex1;
        public int BoneIndex2;
        public int BoneIndex3;
        public int BoneIndex4;
        public float Bone1Weight;
        public float Bone2Weight;
        public float Bone3Weight;
        public float Bone4Weight;
        public BDEF1 AsBDEF1()
        {
            throw new NotImplementedException();
        }

        public BDEF2 AsBDEF2()
        {
            throw new NotImplementedException();
        }

        public BDEF4 AsBDEF4()
        {
            return this;
        }

        public SDEF AsSDEF()
        {
            throw new NotImplementedException();
        }

        public QDEF AsQDEF()
        {
            throw new NotImplementedException();
        }

        public static BDEF4 Read(BinaryReader reader, PmxGlobalData globalData)
        {
            return new BDEF4()
            {
                BoneIndex1 = reader.ReadVarInt(globalData.BoneIndexSize),
                BoneIndex2 = reader.ReadVarInt(globalData.BoneIndexSize),
                BoneIndex3 = reader.ReadVarInt(globalData.BoneIndexSize),
                BoneIndex4 = reader.ReadVarInt(globalData.BoneIndexSize),
                Bone1Weight = reader.ReadSingle(),
                Bone2Weight = reader.ReadSingle(),
                Bone3Weight = reader.ReadSingle(),
                Bone4Weight = reader.ReadSingle(),
            };
        }
    }

    /// <summary>
    /// Spherical deform blending
    /// </summary>
    public struct SDEF : IBoneDeformData
    {
        public int BoneIndex1;
        public int BoneIndex2;
        /// <summary>
        /// Bone 2 weight = 1.0 - Bone 1 weight
        /// </summary>
        public float Bone1Weight;

        public Vec3f C;
        public Vec3f R0;
        public Vec3f R1;
        public BDEF1 AsBDEF1()
        {
            throw new NotImplementedException();
        }

        public BDEF2 AsBDEF2()
        {
            throw new NotImplementedException();
        }

        public BDEF4 AsBDEF4()
        {
            throw new NotImplementedException();
        }

        public SDEF AsSDEF()
        {
            return this;
        }

        public QDEF AsQDEF()
        {
            throw new NotImplementedException();
        }

        public static SDEF Read(BinaryReader reader, PmxGlobalData globalData)
        {
            return new SDEF()
            {
                BoneIndex1 = reader.ReadVarInt(globalData.BoneIndexSize),
                BoneIndex2 = reader.ReadVarInt(globalData.BoneIndexSize),
                Bone1Weight = reader.ReadSingle(),
                C = reader.ReadStruct<Vec3f>(),
                R0 = reader.ReadStruct<Vec3f>(),
                R1 = reader.ReadStruct<Vec3f>(),
            };
        }
    }

    /// <summary>
    /// Dual quaternion deform blending
    /// </summary>
    public struct QDEF : IBoneDeformData
    {
        public int BoneIndex1;
        public int BoneIndex2;
        public int BoneIndex3;
        public int BoneIndex4;
        public float Bone1Weight;
        public float Bone2Weight;
        public float Bone3Weight;
        public float Bone4Weight;
        public BDEF1 AsBDEF1()
        {
            throw new NotImplementedException();
        }

        public BDEF2 AsBDEF2()
        {
            throw new NotImplementedException();
        }

        public BDEF4 AsBDEF4()
        {
            throw new NotImplementedException();
        }

        public SDEF AsSDEF()
        {
            throw new NotImplementedException();
        }

        public QDEF AsQDEF()
        {
            return this;
        }

        public static QDEF Read(BinaryReader reader, PmxGlobalData globalData)
        {
            return new QDEF()
            {
                BoneIndex1 = reader.ReadVarInt(globalData.BoneIndexSize),
                BoneIndex2 = reader.ReadVarInt(globalData.BoneIndexSize),
                BoneIndex3 = reader.ReadVarInt(globalData.BoneIndexSize),
                BoneIndex4 = reader.ReadVarInt(globalData.BoneIndexSize),
                Bone1Weight = reader.ReadSingle(),
                Bone2Weight = reader.ReadSingle(),
                Bone3Weight = reader.ReadSingle(),
                Bone4Weight = reader.ReadSingle(),
            };
        }
    }
}
