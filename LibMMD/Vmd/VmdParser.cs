using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LibMMD.Exceptions;

namespace LibMMD.Vmd
{
    public class VmdParser
    {
        public static Vmd Parse(Stream stream)
        {
            using (var reader = new BinaryReader(stream, Encoding.Default, true))
                return Parse(reader);
        }

        public static Vmd Parse(BinaryReader reader)
        {
            var vmd = new Vmd();
            vmd.Magic = reader.ReadFSString(30, Encoding.ASCII);

            if (vmd.Magic.StartsWith("Vocaloid Motion Data file"))
                vmd.ModelName = reader.ReadFSString(10, Encoding.Unicode);
            else if (vmd.Magic.StartsWith("Vocaloid Motion Data 0002"))
                vmd.ModelName = reader.ReadFSString(20, Encoding.Unicode);
            else
                throw new LibMMDParserException(
                    $"Unknown magic header: {vmd.Magic}, unable to determine model name length");

            vmd.BoneKeyFrames = reader.ReadArray<VmdBoneKeyFrame>(reader.ReadInt32());
            vmd.MorphKeyFrames = reader.ReadArray<VmdMorphKeyFrame>(reader.ReadInt32());
            vmd.CameraKeyFrames = reader.ReadArray<VmdCameraKeyFrame>(reader.ReadInt32());

            if (!reader.IsEndOfStream())
                vmd.LightKeyFrames = reader.ReadArray<VmdLightKeyFrame>(reader.ReadInt32());

            if (!reader.IsEndOfStream())
                vmd.ShadowKeyFrames = reader.ReadArray<VmdShadowKeyFrame>(reader.ReadInt32());

            if (!reader.IsEndOfStream())
            {
                var ikCount = reader.ReadInt32();
                vmd.InverseKinematicKeyFrames = Enumerable.Range(0, ikCount).Select(i => 
                    new VmdInverseKinematicKeyFrame()
                    {
                        FrameIndex = reader.ReadInt32(),
                        Show = reader.ReadByte(),
                        IkInfos = reader.ReadArray<VmdInverseKinematicInfo>(reader.ReadInt32())
                    }).ToArray();
            }
            
            return vmd;
        }
    }
}
