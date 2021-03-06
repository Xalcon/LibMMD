﻿using LibMMD.Exceptions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LibMMD.Tests
{
    [TestFixture]
    public class BinaryReaderLengthPrefixedStringTests
    {
        public static IEnumerable<TestCaseData> ValidTestCases
        {
            get
            {
                yield return new TestCaseData(new byte[] { 0x00, 0x00, 0x00, 0x00 }, Encoding.UTF8)
                    .SetName("TextDecode: Empty String")
                    .Returns("");

                yield return new TestCaseData(new byte[] { 0x0B, 0x00, 0x00, 0x00, 0x48, 0x65, 0x6c, 0x6c, 0x6f, 0x20, 0x57, 0x6f, 0x72, 0x6c, 0x64 }, Encoding.UTF8)
                    .SetName("TextDecode: Utf8 Hello World")
                    .Returns("Hello World");
            }
        }

        public static IEnumerable<TestCaseData> InvalidTestCases
        {
            get
            {
                yield return new TestCaseData(new byte[] { }, Encoding.UTF8)
                    .SetName("TextDecode: Empty Buffer");

                yield return new TestCaseData(new byte[] { 0x00 }, Encoding.UTF8)
                    .SetName("TextDecode: buffer not long enough for len field");

                yield return new TestCaseData(new byte[] { 0x00, 0x00, 0x00, 0x01 }, Encoding.UTF8)
                    .SetName("TextDecode: buffer not long enough for total string length");

                yield return new TestCaseData(BitConverter.GetBytes(-1), Encoding.UTF8)
                    .SetName("TextDecode: Negative length");
            }
        }


        [TestCaseSource(nameof(ValidTestCases))]
        public string DecodeString(byte[] buffer, Encoding encoding)
        {
            using(var stream = new MemoryStream(buffer))
            using(var reader = new BinaryReader(stream))
                return reader.ReadLPString(encoding);
        }

        [TestCaseSource(nameof(InvalidTestCases))]
        public void DecodeErroneousString(byte[] buffer, Encoding encoding)
        {
            using (var stream = new MemoryStream(buffer))
            using (var reader = new BinaryReader(stream))
                Assert.That(new TestDelegate(() => reader.ReadLPString(encoding)), Throws.TypeOf<LibMMDTextDecodingException>());
        }
    }
}
