using System;
using System.Collections.Generic;
using System.Text;
using LibMMD.DataTypes;

namespace LibMMD.Pmx
{
    public class PmxJoint
    {
        public string NameLocal;
        public string NameUniversal;
        public PmxJointType JointType;
        public int RigidBodyIndexA;
        public int RigidBodyIndexB;
        public Vec3f Position;
        public Vec3f Rotation;
        public Vec3f PositionMinimum;
        public Vec3f PositionMaximum;
        public Vec3f RotationMinimum;
        public Vec3f RotationMaximum;
        public Vec3f PositionSpring;
        public Vec3f RotationSpring;
    }

    public enum PmxJointType : byte
    {
        Spring6DOF,
        _6DOF,
        P2P,
        ConeTwist,
        Slider,
        Hinge
    }
}
