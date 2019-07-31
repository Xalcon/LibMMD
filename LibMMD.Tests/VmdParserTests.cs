using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using LibMMD.Vmd;
using NUnit.Framework;

namespace LibMMD.Tests
{
    [TestFixture]
    public unsafe class VmdParserTests
    {
        private static readonly DirectoryInfo BaseDir = new DirectoryInfo($@"{TestContext.CurrentContext.WorkDirectory}\assets");

        public static IEnumerable<TestCaseData> TestCases => BaseDir.Exists ? BaseDir
                .EnumerateFiles("*", SearchOption.AllDirectories)
                .Where(f => f.Name.EndsWith(".vmd") || f.Name.EndsWith(".vmd.gz"))
                .Select(f => new TestCaseData(f)
                    .SetName($"TestParsingMotion: {f.FullName.Substring(BaseDir.FullName.Length + 1)}"))
            : throw new FileNotFoundException($"directory '{BaseDir.FullName}' not found");

        [TestCaseSource(nameof(TestCases))]
        public void TestParsingMotion(FileInfo modelFile)
        {
            Vmd.Vmd motion;
            using (var stream = Open(modelFile))
            {
                motion = VmdParser.Parse(stream);
                TestContext.Out.WriteLine("Bone Key Frames: {0}", motion.BoneKeyFrames.Length);
                TestContext.Out.WriteLine("Morph Key Frames: {0}", motion.MorphKeyFrames.Length);
                TestContext.Out.WriteLine("Camera Key Frames: {0}", motion.CameraKeyFrames.Length);
                TestContext.Out.WriteLine("Light Key Frames: {0}", motion.LightKeyFrames?.Length ?? 0);
                TestContext.Out.WriteLine("Shadow Key Frames: {0}", motion.ShadowKeyFrames?.Length ?? 0);
                
                Warn.If(stream.Position != stream.Length, "File not read to end. Read {0} out of {1} bytes, {2} bytes remaining", stream.Position, stream.Length, stream.Length - stream.Position);
            }

            Assert.That(motion, Is.Not.Null);
        }

        private static Stream Open(FileInfo file)
        {
            if (file.Extension == ".gz")
            {
                var stream = new MemoryStream();
                using(var gzStream = new GZipStream(file.OpenRead(), CompressionMode.Decompress))
                    gzStream.CopyTo(stream);
                stream.Position = 0;
                return stream;
            }

            return file.OpenRead();
        }

        [TestCase(typeof(VmdBoneKeyFrame), 111)]
        [TestCase(typeof(VmdMorphKeyFrame), 23)]
        [TestCase(typeof(VmdCameraKeyFrame), 61)]
        [TestCase(typeof(VmdLightKeyFrame), 28)]
        [TestCase(typeof(VmdShadowKeyFrame), 9)]
        public void TestVmdBoneKeyFrameStructSizes(Type type, int size)
        {
            Assert.That(Marshal.SizeOf(type), Is.EqualTo(size));
        }
    }
}
