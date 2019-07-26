using LibMMD.Exceptions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LibMMD.Tests
{
    [TestFixture]
    public class MMDTextTests
    {
        [TestCaseSource(typeof(TextDecodingTestCaseData), "ValidTestCases")]
        public string DecodeString(byte[] buffer, Encoding encoding)
        {
            using(var stream = new MemoryStream(buffer))
            using(var reader = new BinaryReader(stream))
                return MMDText.Read(reader, encoding);
        }

        [TestCaseSource(typeof(TextDecodingTestCaseData), "InvalidTestCases")]
        public void DecodeErrornousString(byte[] buffer, Encoding encoding)
        {
            using (var stream = new MemoryStream(buffer))
            using (var reader = new BinaryReader(stream))
                Assert.That(new TestDelegate(() => MMDText.Read(reader, encoding)), Throws.TypeOf<LibMMDTextDecodingException>());
        }
    }
}
