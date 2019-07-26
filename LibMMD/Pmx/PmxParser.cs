using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibMMD.DataTypes;
using LibMMD.Exceptions;

namespace LibMMD.Pmx
{
    public class PmxParser
    {
        private const uint PMX_MAGIC = 'P' | 'M' << 8 | 'X' << 16 | ' ' << 24;


        public static PmxModel Parse(Stream stream)
        {
            using (var reader = new BinaryReader(stream))
            {
                return Parse(reader);
            }
        }

        public static PmxModel Parse(BinaryReader reader)
        {
            var pmx = new PmxModel();
            ParseModelInformation(reader, pmx);
            ParseVertexData(reader, pmx);
            ParseIndices(reader, pmx);
            ParseTextures(reader, pmx);
            ParseMaterials(reader, pmx);
            ParseBones(reader, pmx);
            ParseMorphs(reader, pmx);
            ParseDisplayData(reader, pmx);
            ParseRigidBodies(reader, pmx);
            ParseJoints(reader, pmx);
            if (pmx.Version > 2.0)
            {
                ParseSoftBodies(reader, pmx);
            }

            return pmx;
        }

        private static void ParseModelInformation(BinaryReader reader, PmxModel pmx)
        {
            var magic = reader.ReadUInt32();
            if (magic != PMX_MAGIC)
                throw new LibMMDInvalidHeaderException("PMX magic mismatch");

            pmx.Version = reader.ReadSingle();
            var globalsCount = reader.ReadByte();
            pmx.Globals = new PmxGlobalData(reader.ReadBytes(globalsCount).Extend(8));

            var encoding = pmx.Globals.TextEncoding;
            pmx.ModelNameLocal = MMDText.Read(reader, encoding);
            pmx.ModelNameUniversal = MMDText.Read(reader, encoding);
            pmx.ModelCommentsLocal = MMDText.Read(reader, encoding);
            pmx.ModelCommentsUniversal = MMDText.Read(reader, encoding);
        }

        private static void ParseVertexData(BinaryReader reader, PmxModel pmx)
        {
            var vertexCount = reader.ReadInt32();
            pmx.Vertices = new List<PmxVertexData>();
            for (var i = 0; i < vertexCount; i++)
            {
                var vertex = new PmxVertexData();

                vertex.Position = new Vec3f(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                vertex.Normal = new Vec3f(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                vertex.UV = new Vec2f(reader.ReadSingle(), reader.ReadSingle());
                vertex.AdditionalVec4 = new Vec4f[pmx.Globals.AdditionalVec4Count];

                for (var j = 0; j < pmx.Globals.AdditionalVec4Count; j++)
                {
                    vertex.AdditionalVec4[j] = new Vec4f(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                }

                vertex.DeformType = reader.ReadEnum<BoneWeightDeformType>();
                switch (vertex.DeformType)
                {
                    case BoneWeightDeformType.BDEF1:
                        vertex.DeformData = BDEF1.Read(reader, pmx.Globals);
                        break;
                    case BoneWeightDeformType.BDEF2:
                        vertex.DeformData = BDEF2.Read(reader, pmx.Globals);
                        break;
                    case BoneWeightDeformType.BDEF4:
                        vertex.DeformData = BDEF4.Read(reader, pmx.Globals);
                        break;
                    case BoneWeightDeformType.SDEF:
                        vertex.DeformData = SDEF.Read(reader, pmx.Globals);
                        break;
                    case BoneWeightDeformType.QDEF:
                        vertex.DeformData = QDEF.Read(reader, pmx.Globals);
                        break;
                    default:
                        throw new LibMMDParserException($"Unknown deform type: {vertex.DeformType}");
                }

                vertex.EdgeScale = reader.ReadSingle();

                pmx.Vertices.Add(vertex);
            }
        }

        private static void ParseIndices(BinaryReader reader, PmxModel pmx)
        {
            var indexCount = reader.ReadInt32();
            pmx.Indices = new List<int>();
            switch (pmx.Globals.VertexIndexSize)
            {
                case 1:
                    pmx.Indices = reader.ReadArray<byte>(indexCount).Select(x => (int)x).ToList();
                    break;
                case 2:
                    pmx.Indices = reader.ReadArray<ushort>(indexCount).Select(x => (int)x).ToList();
                    break;
                case 4:
                    pmx.Indices = reader.ReadArray<int>(indexCount).ToList();
                    break;
                default:
                    throw new LibMMDParserException($"Unsupported vertex index size: {pmx.Globals.VertexIndexSize}");
            }
        }

        private static void ParseTextures(BinaryReader reader, PmxModel pmx)
        {
            var textureCount = reader.ReadInt32();
            var encoding = pmx.Globals.TextEncoding;
            pmx.Textures = Enumerable.Range(0, textureCount).Select(i => MMDText.Read(reader, encoding)).ToList();
        }

        private static void ParseMaterials(BinaryReader reader, PmxModel pmx)
        {
            var materialsCount = reader.ReadInt32();
            var encoding = pmx.Globals.TextEncoding;
            pmx.Materials = new List<PmxMaterial>();
            for (var i = 0; i < materialsCount; i++)
            {
                var material = new PmxMaterial();
                material.NameLocal = MMDText.Read(reader, encoding);
                material.NameUniversal = MMDText.Read(reader, encoding);
                material.DiffuseColor = reader.ReadStruct<Vec4f>();
                material.SpecularColor = reader.ReadStruct<Vec3f>();
                material.SpecularStrength = reader.ReadSingle();
                material.AmbientColor = reader.ReadStruct<Vec3f>();
                material.DrawingFlags = reader.ReadEnum<PmxMaterialFlags>();
                material.EdgeColor = reader.ReadStruct<Vec4f>();
                material.EdgeScale = reader.ReadSingle();
                material.TextureIndex = reader.ReadVarInt(pmx.Globals.TextureIndexSize);
                material.EnvironmentIndex = reader.ReadVarInt(pmx.Globals.TextureIndexSize);
                material.EnvironmentBlendMode = reader.ReadEnum<EnvironmentBlendMode>();
                material.ToonReference = reader.ReadEnum<ToonReference>();
                material.ToonValue = material.ToonReference == ToonReference.Internal
                    ? reader.ReadByte()
                    : reader.ReadVarInt(pmx.Globals.TextureIndexSize);
                material.MetaData = MMDText.Read(reader, pmx.Globals.TextEncoding);
                material.IndexCount = reader.ReadInt32();
            }
        }

        private static void ParseBones(BinaryReader reader, PmxModel pmx)
        {
            var boneCount = reader.ReadInt32();
            pmx.Bones = new List<PmxBone>();
            for (var i = 0; i < boneCount; i++)
            {
                var bone = new PmxBone();
                bone.NameLocal = MMDText.Read(reader, pmx.Globals.TextEncoding);
                bone.NameUniversal = MMDText.Read(reader, pmx.Globals.TextEncoding);
                bone.Position = reader.ReadStruct<Vec3f>();
                bone.ParentBoneIndex = reader.ReadVarInt(pmx.Globals.BoneIndexSize);
                bone.Layer = reader.ReadInt32();
                bone.Flags = reader.ReadEnum<BoneFlags>();
                
                if (bone.Flags.HasFlag(BoneFlags.IndexedTailPosition))
                    bone.TailPositionBoneIndex = reader.ReadVarInt(pmx.Globals.BoneIndexSize);
                else
                    bone.TailPosition = reader.ReadStruct<Vec3f>();

                if((bone.Flags & (BoneFlags.InheritTranslation | BoneFlags.InheritRotation)) > 0)
                    bone.InheritBone = new InheritBone()
                    {
                        ParentIndex = reader.ReadVarInt(pmx.Globals.BoneIndexSize),
                        ParentWeight = reader.ReadSingle()
                    };

                if (bone.Flags.HasFlag(BoneFlags.FixedAxis))
                    bone.BoneFixedAxis = reader.ReadStruct<Vec3f>();

                if(bone.Flags.HasFlag(BoneFlags.LocalCoordinate))
                    bone.LocalCoordinates = reader.ReadStruct<BoneLocalCoordinates>();

                if (bone.Flags.HasFlag(BoneFlags.ExternalParentDeform))
                    bone.ExternalParent = reader.ReadVarInt(pmx.Globals.BoneIndexSize);

                if(bone.Flags.HasFlag(BoneFlags.InverseKinematics))
                {
                    int linkCount;
                    bone.InverseKinematic = new BoneInverseKinematic()
                    {
                        TargetIndex = reader.ReadVarInt(pmx.Globals.BoneIndexSize),
                        LoopCount = reader.ReadInt32(),
                        LimitRadian = reader.ReadSingle(),
                        LinkCount = (linkCount = reader.ReadInt32()),
                        BoneLinks = Enumerable.Range(0, linkCount).Select(_ =>
                        {
                            var ikLink = new BoneLink
                            {
                                BoneIndex = reader.ReadVarInt(pmx.Globals.BoneIndexSize),
                                HasLimits = reader.ReadBoolean()
                            };

                            if (ikLink.HasLimits)
                            {
                                ikLink.LimitMinimum = reader.ReadStruct<Vec3f>();
                                ikLink.LimitMaximum = reader.ReadStruct<Vec3f>();
                            }

                            return ikLink;
                        }).ToList()
                    };
                }
                pmx.Bones.Add(bone);
            }
        }

        private static void ParseMorphs(BinaryReader reader, PmxModel pmx)
        {
            var morphCount = reader.ReadInt32();
            pmx.Morphs = new List<PmxMorph>();
            for (var i = 0; i < morphCount; i++)
            {
                var morphNameLocal = MMDText.Read(reader, pmx.Globals.TextEncoding);
                var morphNameUniversal = MMDText.Read(reader, pmx.Globals.TextEncoding);
                var panelType = reader.ReadByte();
                var morphType = reader.ReadEnum<PmxMorphType>();
                var morphDataCount = reader.ReadInt32();

                switch (morphType)
                {
                    case PmxMorphType.Group:
                        pmx.Morphs.Add(new PmxMorph<GroupMorph>()
                        {
                            Index = i,
                            NameLocal = morphNameLocal,
                            NameUniversal = morphNameUniversal,
                            PanelType = panelType,
                            MorphType = morphType,
                            Count = morphDataCount,
                            Data = Enumerable.Range(0, morphDataCount).Select(_ => new GroupMorph()
                            {
                                MorphIndex = reader.ReadVarInt(pmx.Globals.MorphIndexSize),
                                Influence = reader.ReadSingle()
                            }).ToList()
                        });
                        break;
                    case PmxMorphType.Vertex:
                        pmx.Morphs.Add(new PmxMorph<VertexMorph>()
                        {
                            Index = i,
                            NameLocal = morphNameLocal,
                            NameUniversal = morphNameUniversal,
                            PanelType = panelType,
                            MorphType = morphType,
                            Count = morphDataCount,
                            Data = Enumerable.Range(0, morphDataCount).Select(_ => new VertexMorph()
                            {
                                VertexIndex = reader.ReadVarInt(pmx.Globals.VertexIndexSize, true),
                                Translation = reader.ReadStruct<Vec3f>()
                            }).ToList()
                        });
                        break;
                    case PmxMorphType.Bone:
                        pmx.Morphs.Add(new PmxMorph<BoneMorph>()
                        {
                            Index = i,
                            NameLocal = morphNameLocal,
                            NameUniversal = morphNameUniversal,
                            PanelType = panelType,
                            MorphType = morphType,
                            Count = morphDataCount,
                            Data = Enumerable.Range(0, morphDataCount).Select(_ => new BoneMorph()
                            {
                                BoneIndex = reader.ReadVarInt(pmx.Globals.BoneIndexSize),
                                Translation = reader.ReadStruct<Vec3f>(),
                                Rotation = reader.ReadStruct<Vec4f>()
                            }).ToList()
                        });
                        break;
                    case PmxMorphType.UV:
                    case PmxMorphType.UVExt1:
                    case PmxMorphType.UVExt2:
                    case PmxMorphType.UVExt3:
                    case PmxMorphType.UVExt4:
                        pmx.Morphs.Add(new PmxMorph<UVMorph>()
                        {
                            Index = i,
                            NameLocal = morphNameLocal,
                            NameUniversal = morphNameUniversal,
                            PanelType = panelType,
                            MorphType = morphType,
                            Count = morphDataCount,
                            Data = Enumerable.Range(0, morphDataCount).Select(_ => new UVMorph()
                            {
                                VertexIndex = reader.ReadVarInt(pmx.Globals.VertexIndexSize, true),
                                Changes = reader.ReadStruct<Vec4f>()
                            }).ToList()
                        });
                        break;
                    case PmxMorphType.Material:
                        pmx.Morphs.Add(new PmxMorph<MaterialMorph>()
                        {
                            Index = i,
                            NameLocal = morphNameLocal,
                            NameUniversal = morphNameUniversal,
                            PanelType = panelType,
                            MorphType = morphType,
                            Count = morphDataCount,
                            Data = Enumerable.Range(0, morphDataCount).Select(_ => new MaterialMorph()
                            {
                                MaterialIndex = reader.ReadVarInt(pmx.Globals.MaterialIndexSize),
                                DiffuseColor = reader.ReadStruct<Vec4f>(),
                                SpecularColor = reader.ReadStruct<Vec3f>(),
                                Specularity = reader.ReadSingle(),
                                AmbientColor = reader.ReadStruct<Vec3f>(),
                                EdgeColor = reader.ReadStruct<Vec4f>(),
                                EdgeSize = reader.ReadSingle(),
                                TextureTint = reader.ReadStruct<Vec4f>(),
                                EnvironmentTint = reader.ReadStruct<Vec4f>(),
                                ToonTint = reader.ReadStruct<Vec4f>()
                            }).ToList()
                        });
                        break;
                    case PmxMorphType.Flip:
                        pmx.Morphs.Add(new PmxMorph<FlipMorph>()
                        {
                            Index = i,
                            NameLocal = morphNameLocal,
                            NameUniversal = morphNameUniversal,
                            PanelType = panelType,
                            MorphType = morphType,
                            Count = morphDataCount,
                            Data = Enumerable.Range(0, morphDataCount).Select(_ => new FlipMorph()
                            {
                                MorphIndex = reader.ReadVarInt(pmx.Globals.MorphIndexSize),
                                Influence = reader.ReadSingle()
                            }).ToList()
                        });
                        break;
                    case PmxMorphType.Impulse:
                        pmx.Morphs.Add(new PmxMorph<ImpulseMorph>()
                        {
                            Index = i,
                            NameLocal = morphNameLocal,
                            NameUniversal = morphNameUniversal,
                            PanelType = panelType,
                            MorphType = morphType,
                            Count = morphDataCount,
                            Data = Enumerable.Range(0, morphDataCount).Select(_ => new ImpulseMorph()
                            {
                                RigidBodyIndex = reader.ReadVarInt(pmx.Globals.RigidBodyIndexSize),
                                LocalFlag = reader.ReadByte(),
                                MovementSpeed = reader.ReadStruct<Vec3f>(),
                                RotationTorque = reader.ReadStruct<Vec3f>()
                            }).ToList()
                        });
                        break;
                    default:
                        throw new LibMMDParserException($"MorphType {morphType} is unknown");
                }
            }
        }

        private static void ParseDisplayData(BinaryReader reader, PmxModel pmx)
        {
            var displayDataCount = reader.ReadInt32();
            pmx.DisplayData = new List<PmxDisplayData>();
            for (var i = 0; i < displayDataCount; i++)
            {
                var frameCount = 0;
                var displayData = new PmxDisplayData()
                {
                    NameLocal = MMDText.Read(reader, pmx.Globals.TextEncoding),
                    NameUniversal = MMDText.Read(reader, pmx.Globals.TextEncoding),
                    IsSpecial = reader.ReadBoolean(),
                    FrameCount = frameCount = reader.ReadInt32(),
                    Frames = Enumerable.Range(0, frameCount).Select<int, FrameData>(_ =>
                    {
                        var type = reader.ReadEnum<PmxDisplayFrameType>();
                        switch (type)
                        {
                            case PmxDisplayFrameType.Bone:
                                return new FrameData<BoneFrameData>()
                                {
                                    FrameType = type,
                                    Data = new BoneFrameData()
                                    {
                                        BoneIndex = reader.ReadVarInt(pmx.Globals.BoneIndexSize)
                                    }
                                };
                            case PmxDisplayFrameType.Morph:
                                return new FrameData<MorphFrameData>()
                                {
                                    FrameType = type,
                                    Data = new MorphFrameData()
                                    {
                                        MorphIndex = reader.ReadVarInt(pmx.Globals.MorphIndexSize)
                                    }
                                };
                            default:
                                throw new LibMMDParserException($"Invalid DisplayFrameType {type}");
                        }
                    }).ToList()
                };
            }
        }

        private static void ParseRigidBodies(BinaryReader reader, PmxModel pmx)
        {
            var rigidBodyCount = reader.ReadInt32();
            pmx.RigidBodies = new List<PmxRigidBody>();
            for (var i = 0; i < rigidBodyCount; i++)
            {
                pmx.RigidBodies.Add(new PmxRigidBody()
                {
                    NameLocal = MMDText.Read(reader, pmx.Globals.TextEncoding),
                    NameUniversal = MMDText.Read(reader, pmx.Globals.TextEncoding),
                    RelatedBoneIndex = reader.ReadVarInt(pmx.Globals.BoneIndexSize),
                    GroupId = reader.ReadByte(),
                    NonCollisionMask = reader.ReadUInt16(),
                    Shape = reader.ReadEnum<PmxRigidBodyShapeType>(),
                    ShapeSize = reader.ReadStruct<Vec3f>(),
                    ShapePosition = reader.ReadStruct<Vec3f>(),
                    ShapeRotation = reader.ReadStruct<Vec3f>(),
                    Mass = reader.ReadSingle(),
                    MoveAttenuation = reader.ReadSingle(),
                    RotationDampening = reader.ReadSingle(),
                    Repulsion = reader.ReadSingle(),
                    FrictionForce = reader.ReadSingle(),
                    PhysicsMode = reader.ReadEnum<PmxRigidBodyPhysicsMode>()
                });
            }
        }

        private static void ParseJoints(BinaryReader reader, PmxModel pmx)
        {
            var jointCount = reader.ReadInt32();
            pmx.Joints = new List<PmxJoint>();
            for (var i = 0; i < jointCount; i++)
            {
                pmx.Joints.Add(new PmxJoint()
                {
                    NameLocal = MMDText.Read(reader, pmx.Globals.TextEncoding),
                    NameUniversal = MMDText.Read(reader, pmx.Globals.TextEncoding),
                    JointType = reader.ReadEnum<PmxJointType>(),
                    RigidBodyIndexA = reader.ReadVarInt(pmx.Globals.RigidBodyIndexSize),
                    RigidBodyIndexB = reader.ReadVarInt(pmx.Globals.RigidBodyIndexSize),
                    Position = reader.ReadStruct<Vec3f>(),
                    Rotation = reader.ReadStruct<Vec3f>(),
                    PositionMinimum = reader.ReadStruct<Vec3f>(),
                    PositionMaximum = reader.ReadStruct<Vec3f>(),
                    RotationMinimum = reader.ReadStruct<Vec3f>(),
                    RotationMaximum = reader.ReadStruct<Vec3f>(),
                    PositionSpring = reader.ReadStruct<Vec3f>(),
                    RotationSpring = reader.ReadStruct<Vec3f>()
                });
            }
        }

        private static void ParseSoftBodies(BinaryReader reader, PmxModel pmx)
        {
            var softBodyCount = reader.ReadInt32();
            pmx.SoftBodies = new List<PmxSoftBody>();
            for (var i = 0; i < softBodyCount; i++)
            {
                pmx.SoftBodies.Add(new PmxSoftBody()
                {
                    
                });
            }
        }
    }
}
