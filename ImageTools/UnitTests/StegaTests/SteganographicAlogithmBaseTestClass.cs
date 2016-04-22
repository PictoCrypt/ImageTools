using System;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using FunctionLib.Cryptography;
using FunctionLib.CustomException;
using FunctionLib.Helper;
using FunctionLib.Model;
using FunctionLib.Steganography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.CryptTests.Base;

namespace UnitTests.StegaTests
{
    public abstract class SteganographicAlogithmBaseTestClass
    {
        protected SteganographicAlgorithmImpl Algorithm;

        private TimeSpan mDecryptionTime;
        private TimeSpan mEncryptionTime;
        private Stopwatch mStopwatch;

        [TestInitialize]
        public virtual void Initialize()
        {
            mStopwatch = new Stopwatch();
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            WriteToOutput();
        }

        [ClassCleanup]
        public void CleanUp()
        {
            FileManager.GetInstance().CleanUp();
        }

        [TestMethod]
        public void FunctionTextTest()
        {
            var encrypted = Encode(GetTheRightImage(), TestingConstants.NormalText);
            Assert.IsFalse(string.IsNullOrEmpty(encrypted));
            Assert.IsTrue(File.Exists(encrypted));

            var decoded = Decode(encrypted);
            Assert.IsFalse(string.IsNullOrEmpty(decoded));
        }

        [TestMethod]
        public void FunctionImageTest()
        {
            var encrypted = Encode(GetTheRightImage(), TestingConstants.SmallImage);
            Assert.IsFalse(string.IsNullOrEmpty(encrypted));
            Assert.IsTrue(File.Exists(encrypted));

            var decoded = Decode(encrypted);
            Assert.IsFalse(string.IsNullOrEmpty(decoded));
            Assert.IsTrue(File.Exists(decoded));
        }

        [TestMethod]
        public void FunctionTextWithCompressionTest()
        {
            var encrypted = Encode(GetTheRightImage(), TestingConstants.NormalText, null, null, true);
            Assert.IsFalse(string.IsNullOrEmpty(encrypted));
            Assert.IsTrue(File.Exists(encrypted));

            var decoded = Decode(encrypted, null, null, true);
            Assert.IsFalse(string.IsNullOrEmpty(decoded));
        }

        [TestMethod]
        public void FunctionImageWithCompressionTest()
        {
            var encrypted = Encode(GetTheRightImage(), TestingConstants.SmallImage, null, null, true);
            Assert.IsFalse(string.IsNullOrEmpty(encrypted));
            Assert.IsTrue(File.Exists(encrypted));

            var decoded = Decode(encrypted, null, null, true);
            Assert.IsFalse(string.IsNullOrEmpty(decoded));
            Assert.IsTrue(File.Exists(decoded));
        }

        [TestMethod]
        public void FunctionDocTest()
        {
            var encrypted = Encode(GetTheRightImage(), TestingConstants.NormalTestdoc);
            Assert.IsFalse(string.IsNullOrEmpty(encrypted));
            Assert.IsTrue(File.Exists(encrypted));

            var decoded = Decode(encrypted);
            Assert.IsFalse(string.IsNullOrEmpty(decoded));
            Assert.IsTrue(File.Exists(decoded));
        }

        [TestMethod]
        public void FunctionTextWithLsbTest()
        {
            var encrypted = Encode(GetTheRightImage(), TestingConstants.NormalText, null, null, false, 4);
            Assert.IsFalse(string.IsNullOrEmpty(encrypted));
            Assert.IsTrue(File.Exists(encrypted));

            var decoded = Decode(encrypted, null, null, false, 4);
            Assert.IsFalse(string.IsNullOrEmpty(decoded));
        }

        [TestMethod]
        public void FunctionFileWithLsbTest()
        {
            var encrypted = Encode(GetTheRightImage(), TestingConstants.SmallImage, null, null, false, 4);
            Assert.IsFalse(string.IsNullOrEmpty(encrypted));
            Assert.IsTrue(File.Exists(encrypted));

            var decoded = Decode(encrypted, null, null, false, 4);
            Assert.IsFalse(string.IsNullOrEmpty(decoded));
            Assert.IsTrue(File.Exists(decoded));
        }

        [TestMethod]
        [ExpectedException(typeof(ContentLengthException))]
        public virtual void EncodeTextWithoutSpaceTest()
        {
            var encrypted = Encode(GetTheRightImage(true), TestingConstants.LongText);
        }

        [TestMethod]
        [ExpectedException(typeof(ContentLengthException))]
        public virtual void EncodeDocumentWithoutSpaceTest()
        {
            var encrypted = Encode(GetTheRightImage(true), TestingConstants.LargeTestdoc);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EncodeWithoutMessageTest()
        {
            var encrypted = Encode(GetTheRightImage(), string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EncodeWithoutCoverTest()
        {
            var encrypted = Encode(null, TestingConstants.NormalText);
        }

        [TestMethod]
        [ExpectedException(typeof(BadImageFormatException))]
        public virtual void EncodeWithWrongImageFormatTest()
        {
            if (Algorithm.PossibleImageFormats.Contains(ImageFormat.Jpeg))
            {
                var encrypted = Encode(TestingConstants.NormalImage, TestingConstants.NormalText);
            }
            else
            {
                var encrypted = Encode(TestingConstants.NormalJpgImage, TestingConstants.NormalText);
            }
        }

        [TestMethod]
        public void FunctionCryptedTextTest()
        {
            var encrypted = Encode(GetTheRightImage(), TestingConstants.NormalText, new AesAlgorithm(),
                TestingConstants.Password);
            Assert.IsFalse(string.IsNullOrEmpty(encrypted));
            Assert.IsTrue(File.Exists(encrypted));

            var decoded = Decode(encrypted, new AesAlgorithm(), TestingConstants.Password);
            Assert.IsFalse(string.IsNullOrEmpty(decoded));
        }

        [TestMethod]
        public void FunctionCryptedFileTest()
        {
            var encrypted = Encode(GetTheRightImage(), TestingConstants.SmallImage, new AesAlgorithm(),
                TestingConstants.Password);
            Assert.IsFalse(string.IsNullOrEmpty(encrypted));
            Assert.IsTrue(File.Exists(encrypted));

            var decoded = Decode(encrypted, new AesAlgorithm(), TestingConstants.Password);
            Assert.IsFalse(string.IsNullOrEmpty(decoded));
            Assert.IsTrue(File.Exists(decoded));
        }

        private void WriteToOutput()
        {
            Trace.WriteLine(GetType() + "- TEST");
            Trace.WriteLine("Encryption-Time: " + mEncryptionTime);
            Trace.WriteLine("Decryption-Time: " + mDecryptionTime);
            Trace.WriteLine("");
            Trace.WriteLine("");
        }

        private string GetTheRightImage(bool small = false)
        {
            if (Algorithm.PossibleImageFormats.Contains(ImageFormat.Jpeg))
            {
                if (!small)
                {
                    return TestingConstants.NormalJpgImage;
                }
                return TestingConstants.SmallJpgImage;
            }
            if (!small)
            {
                return TestingConstants.NormalImage;
            }
            return TestingConstants.SmallImage;
        }

        private string Encode(string src, string message, CryptographicAlgorithmImpl crypt = null,
            string password = null, bool compression = false, int lsbIndicator = 3)
        {
            var model = new EncodeModel(src, message, crypt, password, Algorithm, compression, lsbIndicator);
            mStopwatch.Start();
            var result = model.Encode();
            mStopwatch.Stop();
            mEncryptionTime = mStopwatch.Elapsed;
            mStopwatch.Reset();
            return result;
        }

        private string Decode(string src, CryptographicAlgorithmImpl crypto = null, string password = null,
            bool compression = false,
            int lsbIndicator = 3)
        {
            var model = new DecodeModel(src, crypto, password, Algorithm, compression, lsbIndicator);
            mStopwatch.Start();
            var result = model.Decode();
            mStopwatch.Stop();
            mDecryptionTime = mStopwatch.Elapsed;
            mStopwatch.Reset();
            return result;
        }
    }
}