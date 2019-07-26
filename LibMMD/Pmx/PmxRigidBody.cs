using System;
using System.Collections.Generic;
using System.Text;
using LibMMD.DataTypes;

namespace LibMMD.Pmx
{
    public class PmxRigidBody
    {
        public string NameLocal;
        public string NameUniversal;
        public int RelatedBoneIndex;
        public byte GroupId;
        public ushort NonCollisionMask;
        public PmxRigidBodyShapeType Shape;
        public Vec3f ShapeSize;
        public Vec3f ShapePosition;
        public Vec3f ShapeRotation;
        public float Mass;
        public float MoveAttenuation;
        public float RotationDampening;
        public float Repulsion;
        public float FrictionForce;
        public PmxRigidBodyPhysicsMode PhysicsMode;
    }

    public enum PmxRigidBodyShapeType : byte
    {
        Sphere = 0,
        Box = 1,
        Capsule = 2
    }

    public enum PmxRigidBodyPhysicsMode : byte
    {
        FollowBone = 0,
        Physics,
        PhysicsAndBone
    }
}
