using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using FunctionLib.CustomException;
using FunctionLib.Helper;
using FunctionLib.Model.Message;
using FunctionLib.Steganography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        public void EncodeText()
        {
            var encrypted = Encode(TestingConstants.NormalImage, new TextMessage(TestingConstants.NormalText));

            var decrypted = Decode(encrypted);
            var result = decrypted.ConvertBack();
            Assert.IsTrue(result.StartsWith(TestingConstants.NormalText));
        }

        [TestMethod]
        public string Encoding()
        {
            var encrypted = Encode(TestingConstants.NormalImage, new TextMessage(TestingConstants.NormalText),
                    TestingConstants.Password.GetHashCode());
            Assert.IsTrue(!string.IsNullOrEmpty(encrypted));
            using (var bmp = new Bitmap(encrypted))
            {
                Assert.IsNotNull(bmp);
            }
            return encrypted;
        }

        [TestMethod]
        public void Decodig()
        {
            var enc = Encoding();
            var decrypted = Decode(enc, TestingConstants.Password.GetHashCode());
            var result = decrypted.ConvertBack();
            Assert.IsTrue(result.Length > 0);
        }

        [TestMethod]
        public void EncodeWithLsbIndicator()
        {
            var encrypted = Encode(TestingConstants.NormalImage, new TextMessage(TestingConstants.NormalText),
                TestingConstants.Password.GetHashCode(), 4);
            var decrypted = Decode(encrypted, TestingConstants.Password.GetHashCode(), 4);

            var result = decrypted.ConvertBack();
            Assert.IsTrue(result.StartsWith(TestingConstants.NormalText));
        }

        [TestMethod]
        public void EncodeImage()
        {
            var encrypted = Encode(TestingConstants.NormalImage, new DocumentMessage(TestingConstants.SmallImage),
                TestingConstants.Password.GetHashCode());

            var decrypted = Decode(encrypted, TestingConstants.Password.GetHashCode());
            var result = decrypted.ConvertBack();
            Assert.IsNotNull(result);
            Assert.IsTrue(File.Exists(result));
        }

        [TestMethod]
        public void EncodeDocument()
        {
            var encrypted = Encode(TestingConstants.NormalImage, new DocumentMessage(TestingConstants.Testdoc),
                TestingConstants.Password.GetHashCode());
            var decrypted = Decode(encrypted, TestingConstants.Password.GetHashCode());
            var result = decrypted.ConvertBack();
            Assert.IsNotNull(result);
            Assert.IsTrue(File.Exists(result));
        }

        [TestMethod]
        [ExpectedException(typeof (ContentLengthException))]
        public void EncodeWithoutSpace()
        {
            var encrypted = Encode(TestingConstants.SmallKoala, new TextMessage(TestingConstants.LongText),
                    TestingConstants.Password.GetHashCode());
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void EncodeWithoutText()
        {
            var encrypted = Encode(TestingConstants.NormalImage, new TextMessage(string.Empty));
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void EncodeWithoutCover()
        {
            var encrypted = Encode(null, new TextMessage(TestingConstants.NormalText));
        }

        private ISecretMessage Decode(string encrypted)
        {
            mStopwatch.Start();
            var decrypted = Decode(encrypted, TestingConstants.Password.GetHashCode());
            mStopwatch.Stop();
            mDecryptionTime = mStopwatch.Elapsed;
            Assert.IsFalse(decrypted == null);
            return decrypted;
        }

        private string Encode(string src, ISecretMessage value)
        {
            mStopwatch.Start();
            var encrypted = Encode(src, value, TestingConstants.Password.GetHashCode());
            mStopwatch.Stop();
            mEncryptionTime = mStopwatch.Elapsed;
            mStopwatch.Reset();
            Assert.IsTrue(encrypted != null);
            return encrypted;
        }

        private void WriteToOutput()
        {
            Trace.WriteLine(GetType() + "- TEST");
            Trace.WriteLine("Encryption-Time: " + mEncryptionTime);
            Trace.WriteLine("Decryption-Time: " + mDecryptionTime);
            Trace.WriteLine("");
            Trace.WriteLine("");
        }

        private string Encode(string src, ISecretMessage value, int password, int lsbIndicator = 3)
        {
            return Algorithm.Encode(src, value, password, lsbIndicator);
        }

        private ISecretMessage Decode(string src, int password, int lsbIndicator = 3)
        {
            return Algorithm.Decode(src, password, lsbIndicator);
        }
    }
}