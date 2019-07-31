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
        public static void ExportModel(PmxModel pmx, Stream outputStream, string materialFileName)
        {
            var oldCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            try
            {
                var writer = new StreamWriter(outputStream);
                writer.Write("# Model: ");
                writer.WriteLine(pmx.ModelNameLocal);

                writer.WriteLine($"mtllib {materialFileName}");

                writer.WriteLine($"# vertex positions");
                writer.WriteLine($"o {pmx.ModelNameLocal}");
                writer.WriteLine($"g {pmx.ModelNameLocal}");
                foreach (var vertex in pmx.Vertices)
                    writer.WriteLine($"v {vertex.Position.X} {vertex.Position.Y} {vertex.Position.Z}");

                writer.WriteLine($"# vertex normals");
                foreach (var vertex in pmx.Vertices)
                    writer.WriteLine($"vn {vertex.Normal.X} {vertex.Normal.Y} {vertex.Normal.Z}");

                writer.WriteLine($"# vertex uv");
                foreach (var vertex in pmx.Vertices)
                    writer.WriteLine($"vt {vertex.UV.X} {1 - vertex.UV.Y}");


                writer.WriteLine($"# polygonal faces");
                var mat = pmx.Materials.First();
                var matIndex = 0;
                var materialIndexCountOffset = 0;
                writer.WriteLine($"g group_mat{pmx.Materials.IndexOf(mat)}");
                writer.WriteLine($"usemtl mat{pmx.Materials.IndexOf(mat)}");
                for (var i = 0; i < pmx.Indices.Count; i += 3)
                {
                    var idx0 = pmx.Indices[i] + 1;
                    var idx1 = pmx.Indices[i + 1] + 1;
                    var idx2 = pmx.Indices[i + 2] + 1;
                    if (mat.IndexCount + materialIndexCountOffset < i)
                    {
                        materialIndexCountOffset += mat.IndexCount;
                        mat = pmx.Materials[++matIndex];
                        writer.WriteLine($"g group_mat{pmx.Materials.IndexOf(mat)}");
                        writer.WriteLine($"usemtl mat{pmx.Materials.IndexOf(mat)}");
                    }
                    writer.WriteLine($"f {idx0}/{idx0}/{idx0} {idx1}/{idx1}/{idx1} {idx2}/{idx2}/{idx2}");
                }
                writer.Flush();
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = oldCulture;
            }
        }

        public static void ExportMaterial(PmxModel pmx, Stream outputStream)
        {
            var oldCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            try
            {
                using (var writer = new StreamWriter(outputStream, Encoding.Default, 2048, true))
                {
                    foreach (var mat in pmx.Materials)
                    {
                        writer.WriteLine($"newmtl mat{pmx.Materials.IndexOf(mat)}");
                        writer.WriteLine($"Ka {mat.AmbientColor.X} {mat.AmbientColor.Y} {mat.AmbientColor.Z}");
                        writer.WriteLine($"Kd {mat.DiffuseColor.X} {mat.DiffuseColor.Y} {mat.DiffuseColor.Z}");
                        //writer.WriteLine($"Ks {mat.SpecularColor.X} {mat.SpecularColor.Y} {mat.SpecularColor.Z}");
                        //writer.WriteLine($"Ns {mat.SpecularStrength}");
                        //writer.WriteLine($"d {mat.DiffuseColor.W}");
                        //writer.WriteLine($"Tr {1 - mat.DiffuseColor.W}");
                        //writer.WriteLine($"illum 2");
                        if(mat.TextureIndex > 0)
                        {
                            writer.WriteLine($"map_Ka {pmx.Textures[mat.TextureIndex].Replace("\\", "/")}");
                            writer.WriteLine($"map_Kd {pmx.Textures[mat.TextureIndex].Replace("\\", "/")}");
                        }
                        writer.WriteLine();
                        //writer.WriteLine($"map_Ks {pmx.Textures[mat.TextureIndex]}");
                    }
                }
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = oldCulture;
            }
        }
    }
}
