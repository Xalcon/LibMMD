using System;
using System.Collections.Generic;
using System.Text;
using LibMMD.DataTypes;

namespace LibMMD.Pmx
{
    public struct PmxBone
    {
        public string NameLocal;
        public string NameUniversal;
        public Vec3f Position;
        public int ParentBoneIndex;
        public int Layer;
        public BoneFlags Flags;

        public int? TailPositionBoneIndex;
        public Vec3f? TailPosition;
        
        public InheritBone? InheritBone;

        public Vec3f? BoneFixedAxis;

        public BoneLocalCoordinates? LocalCoordinates;

        public int? ExternalParent;

        public BoneInverseKinematic? InverseKinematic;
    }

    public struct InheritBone
    {
        public int ParentIndex;
        public float ParentWeight;
    }

    public struct BoneLocalCoordinates
    {
        public Vec3f VectorX;
        public Vec3f VectorZ;
    }

    public struct BoneInverseKinematic
    {
        public int TargetIndex;
        public int LoopCount;
        public float LimitRadian;
        public int LinkCount;
        public List<BoneLink> BoneLinks;
    }

    public struct BoneLink
    {
        public int BoneIndex;
        public bool HasLimits;
        public Vec3f LimitMinimum;
        public Vec3f LimitMaximum;
    }

    [Flags]
    public enum BoneFlags : short
    {
        IndexedTailPosition = 1 << 0, // Is the tail position a vec3 or bone index
        Rotatable = 1 << 1, // Enables rotation
        Translatable = 1 << 2, // Enables translation (shear)
        IsVisible = 1 << 3, // ???
        Enabled = 1 << 4, // ???
        InverseKinematics = 1 << 5, // Use inverse kinematics (physics)
        InheritRotation = 1 << 6, // Rotation inherits from another bone
        InheritTranslation = 1 << 7, // Translation inherits from another bone
        FixedAxis = 1 << 8, // The bone's shaft is fixed in a direction
        LocalCoordinate = 1 << 9, // ???
        PhysicsAfterDeform = 1 << 10, // ???
        ExternalParentDeform = 1 << 11, // ???
    }
}
