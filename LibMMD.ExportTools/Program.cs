using System;
using System.IO;
using CommandLine;
using LibMMD.ExportTools.Wavefront;
using LibMMD.Pmx;

namespace LibMMD.ExportTools
{
    class Program
    {
        [Verb("wavefront", HelpText = "Export a pmx model into a wavefront obj")]
        class WavefrontExportOptions
        {
            [Option('i', "input", Required = true)]
            public string InputFilePath { get; set; }
            [Option('o', "output", Required = false)]
            public string OutputFilePath { get; set; }
        }

        static int Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<WavefrontExportOptions>(args)
                .WithParsed<WavefrontExportOptions>(ExportWavefrontModel);

            return result.Tag == ParserResultType.Parsed ? 0 : -1;
        }

        private static void ExportWavefrontModel(WavefrontExportOptions obj)
        {
            var file = new FileInfo(obj.InputFilePath);
            if (!file.Exists)
            {
                Console.Error.WriteLine("File not found: " + file.FullName);
            }

            using (var stream = file.OpenRead())
            using (var outputStream = File.OpenWrite(file.FullName + ".obj"))
            using (var mtlOutputStream = File.OpenWrite(file.FullName + ".mtl"))
            {
                var model = PmxParser.Parse(stream);
                PmxToWavefrontExporter.ExportModel(model, outputStream, file.FullName + ".mtl");
                PmxToWavefrontExporter.ExportMaterial(model, mtlOutputStream);
            }
        }
    }
}
