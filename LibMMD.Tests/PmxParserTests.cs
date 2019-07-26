using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LibMMD.Pmx;
using NUnit.Framework;

namespace LibMMD.Tests
{
    [TestFixture]
    public class PmxParserTests
    {
        [TestCase("assets/Model.pmx")]
        public void TestParsingModel(string modelPath)
        {
            PmxModel model;
            using (var stream = File.OpenRead(modelPath))
                model = PmxParser.Parse(stream);

            Assert.That(model, Is.Not.Null);
        }
    }
}
