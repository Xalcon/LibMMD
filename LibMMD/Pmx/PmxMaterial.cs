using System;
using System.Collections.Generic;
using System.Text;
using LibMMD.DataTypes;

namespace LibMMD.Pmx
{
    public struct PmxMaterial
    {
        /// <summary>
        /// Handy name for the material (Usually Japanese)
        /// </summary>
        public string NameLocal;
        /// <summary>
        /// Handy name for the material (Usually English)
        /// </summary>
        public string NameUniversal;
        /// <summary>
        /// RGBA colour (Alpha will set a semi-transparent material)
        /// </summary>
        public Vec4f DiffuseColor;
        /// <summary>
        /// RGB colour of the reflected light
        /// </summary>
        public Vec3f SpecularColor;
        /// <summary>
        /// The "size" of the specular highlight
        /// </summary>
        public float SpecularStrength;
        /// <summary>
        /// RGB color of the material shadow (When out of light)
        /// </summary>
        public Vec3f AmbientColor;
        public PmxMaterialFlags DrawingFlags;
        /// <summary>
        /// RGBA color of the pencil-outline edge (Alpha for semi-transparent)
        /// </summary>
        public Vec4f EdgeColor;
        /// <summary>
        /// Pencil-outline scale (1.0 should be around 1 pixel)
        /// </summary>
        public float EdgeScale;
        public int TextureIndex;
        /// <summary>
        /// Same as texture index, but for environment mapping
        /// </summary>
        public int EnvironmentIndex;
        public EnvironmentBlendMode EnvironmentBlendMode;
        public ToonReference ToonReference;
        /// <summary>
        /// Behaviour depends on Toon reference value
        /// Toon value will be a texture index much like the standard texture and
        /// environment texture indexes unless the Toon reference byte is equal to 1,
        /// in which case Toon value will be a byte that references a set of 10
        /// internal toon textures (Most implementations will use "toon01.bmp"
        /// to "toon10.bmp" as the internal textures, see the reserved names for
        /// Textures above).
        /// </summary>
        public int ToonValue;
        /// <summary>
        /// This is used for scripting or additional data
        /// </summary>
        public string MetaData;
        /// <summary>
        /// How many surfaces this material affects.
        /// Will always be a multiple of 3.
        /// It is based on the offset of the previous material through to the size
        /// of the current material. If you add up all the surface counts for all
        /// materials you should end up with the total number of surfaces.
        /// </summary>
        public int IndexCount;
    }

    public enum ToonReference : byte
    {
        Texture = 0,
        Internal
    }

    public enum EnvironmentBlendMode : byte
    {
        Disabled = 0,
        Multiply,
        Additive,
        AdditionalVec4
    }

    [Flags]
    public enum PmxMaterialFlags : byte
    {
        /// <summary>
        /// Disables back-face culling
        /// </summary>
        NoCull = 1 << 0,
        /// <summary>
        /// Projects a shadow onto the geometry
        /// </summary>
        GroundShadow = 1 << 1,
        /// <summary>
        /// Renders to the shadow map
        /// </summary>
        DrawShadow = 1 << 2,
        /// <summary>
        /// Receives a shadow from the shadow map	
        /// </summary>
        ReceiveShadow = 1 << 3,
        /// <summary>
        /// Has "pencil" outline
        /// </summary>
        HasEdge = 1 << 4,
        /// <summary>
        /// Uses additional vec4 1 for vertex color
        /// </summary>
        VertexColor = 1 << 5,
        /// <summary>
        /// Each of the 3 vertices are points
        /// </summary>
        PointDrawing = 1 << 6,
        /// <summary>
        /// The triangle is rendered as lines
        /// </summary>
        LineDrawing = 1 << 7
    }
}
