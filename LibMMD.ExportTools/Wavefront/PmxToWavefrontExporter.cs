using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using LibMMD.Pmx;

namespace LibMMD.ExportTools.Wavefront
{
    internal class PmxToWavefrontExporter
    {
        public static void ExportModel(PmxModel pmx, Stream outputStream)
        {
            var oldCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            try
            {
                var writer = new StreamWriter(outputStream);
                writer.Write("# Model: ");
                writer.WriteLine(pmx.ModelNameLocal);
            
                writer.WriteLine($"# vertex positions");
                foreach(var vertex in pmx.Vertices)
                    writer.WriteLine($"v {vertex.Position.X} {vertex.Position.Y} {vertex.Position.Z}");

                writer.WriteLine($"# vertex normals");
                foreach(var vertex in pmx.Vertices)
                    writer.WriteLine($"vn {vertex.Normal.X} {vertex.Normal.Y} {vertex.Normal.Z}");
            
                writer.WriteLine($"# vertex uv");
                foreach(var vertex in pmx.Vertices)
                    writer.WriteLine($"vt {vertex.UV.X} {vertex.UV.Y}");

                
                writer.WriteLine($"# polygonal faces");
                var mat = pmx.Materials.First();
                var matIndex = 0;
                var materialIndexCountOffset = 0;
                writer.WriteLine($"# usemtl {mat.NameLocal}");
                for (var i = 0; i < pmx.Indices.Count; i += 3)
                {
                    var idx0 = pmx.Indices[i];
                    var idx1 = pmx.Indices[i + 1];
                    var idx2 = pmx.Indices[i + 2];
                    if (mat.IndexCount + materialIndexCountOffset < i)
                    {
                        materialIndexCountOffset += mat.IndexCount;
                        mat = pmx.Materials[++matIndex];
                        writer.WriteLine($"# usemtl {mat.NameLocal}");
                    }
                    writer.WriteLine($"f {idx0}/{idx0}/{idx0} {idx1}/{idx1}/{idx1} {idx2}/{idx2}/{idx2}");
                }
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = oldCulture;
            }
        }

        public static void ExportMaterial(PmxModel pmx, Stream outputStream)
        {

        }
    }
}
