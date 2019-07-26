using System;
using System.Collections.Generic;
using System.Text;

namespace LibMMD.Pmx
{
    public class PmxSoftBody
    {
        public string NameLocal;
        public string NameUniversal;
        public PmxSoftBodyShapeType ShapeType;
        public int MaterialIndex;
        public byte GroupId;
        public ushort NoCollisionGroup;
        public PmxSoftBodyFlags Flags;
        public int BLinkCreateDistance;
        public int NumberOfClusters;
        public float TotalMass;
        public float CollisionMargin;
        public PmxSoftbodyAerodynamicsModel AerodynamicsModel;
        ///<summary>
        /// Velocities correction factor (Baumgarte)
        /// </summary>
        public float ConfigVCF; 
        ///<summary>
        /// Damping coefficient
        /// </summary>
        public float ConfigDP; 
        ///<summary>
        /// Drag coefficient
        /// </summary>
        public float ConfigDG; 
        ///<summary>
        /// Lift coefficient
        /// </summary>
        public float ConfigLF; 
        ///<summary>
        /// Pressure coefficient
        /// </summary>
        public float ConfigPR; 
        ///<summary>
        /// Volume conversation coefficient
        /// </summary>
        public float ConfigVC; 
        ///<summary>
        /// Dynamic friction coefficient
        /// </summary>
        public float ConfigDF; 
        ///<summary>
        /// Pose matching coefficient
        /// </summary>
        public float ConfigMT; 
        ///<summary>
        /// Rigid contacts hardness
        /// </summary>
        public float ConfigCHR; 
        ///<summary>
        /// Kinetic contacts hardness
        /// </summary>
        public float ConfigKHR; 
        ///<summary>
        /// Soft contacts hardness
        /// </summary>
        public float ConfigSHR; 
        ///<summary>
        /// Anchors hardness
        /// </summary>
        public float ConfigAHR; 
        ///<summary>
        /// Soft vs rigid hardness
        /// </summary>
        public float ClusterSRHR_CL; 
        ///<summary>
        /// Soft vs kinetic hardness
        /// </summary>
        public float ClusterSKHR_CL; 
        ///<summary>
        /// Soft vs soft hardness
        /// </summary>
        public float ClusterSSHR_CL; 
        ///<summary>
        /// Soft vs rigid impulse split
        /// </summary>
        public float ClusterSR_SPLT_CL; 
        ///<summary>
        /// Soft vs kinetic impulse split
        /// </summary>
        public float ClusterSK_SPLT_CL; 
        ///<summary>
        /// Soft vs soft impulse split
        /// </summary>
        public float ClusterSS_SPLT_CL; 
        ///<summary>
        /// Velocities solver iterations
        /// </summary>
        public int InterationV_IT; 
        ///<summary>
        /// Positions solver iterations
        /// </summary>
        public int InterationP_IT; 
        ///<summary>
        /// Drift solver iterations
        /// </summary>
        public int InterationD_IT; 
        ///<summary>
        /// Cluster solver iterations
        /// </summary>
        public int InterationC_IT; 
        ///<summary>
        /// Linear stiffness coefficient
        /// </summary>
        public int MaterialLST; 
        ///<summary>
        /// Area / Angular stiffness coefficient
        /// </summary>
        public int MaterialAST; 
        ///<summary>
        /// Volume stiffness coefficient
        /// </summary>
        public int MaterialVST;

        public int AnchorRigidBodyCount;
        public List<PmxSoftBodyAnchorRigidBody> AnchorRigidBodies;
        public int VertexPintCount;
        public List<PmxSoftBodyVertexPin> VertexPins;
    }

    public enum PmxSoftBodyShapeType : byte
    {
        TriMesh = 0,
        Rope = 1
    }

    [Flags]
    public enum PmxSoftBodyFlags : byte
    {
        BLink = 1 << 0,
        ClusterCreation = 1 << 1,
        LinkCrossing = 1 << 2
    }

    public enum PmxSoftbodyAerodynamicsModel : int
    {
        VPoint,
        VTwoSided,
        VOneSided,
        FOneSided,
        FTwoSided
    }

    public class PmxSoftBodyAnchorRigidBody
    {
        public int RigidBodyIndex;
        public int VertexIndex;
        public byte NearMode;
    }

    public class PmxSoftBodyVertexPin
    {
        public int VertexIndex;
    }


}
