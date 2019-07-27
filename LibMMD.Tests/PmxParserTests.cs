using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LibMMD.Pmx;
using NUnit.Framework;

namespace LibMMD.Tests
{
    [TestFixture]
    public class PmxParserTests
    {
        private static readonly DirectoryInfo BaseDir = new DirectoryInfo(@"assets");

        public static IEnumerable<TestCaseData> PmxTestCases => BaseDir
            .EnumerateFiles("*.pmx", SearchOption.AllDirectories)
            .Select(f => new TestCaseData(f)
                .SetName($"TestParsingModel: {f.FullName.Substring(BaseDir.FullName.Length + 1)}"));

        [TestCaseSource("PmxTestCases")]
        public void TestParsingModel(FileInfo modelFile)
        {
            PmxModel model;
            using (var stream = modelFile.OpenRead())
                model = PmxParser.Parse(stream);

            Assert.That(model, Is.Not.Null);
        }
    }
}
