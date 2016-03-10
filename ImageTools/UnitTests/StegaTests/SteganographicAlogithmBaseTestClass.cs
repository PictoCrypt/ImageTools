using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using FunctionLib.CustomException;
using FunctionLib.Helper;
using FunctionLib.Model.Message;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.StegaTests
{
    public abstract class SteganographicAlogithmBaseTestClass
    {
        private TimeSpan mDecryptionTime;
        private TimeSpan mEncryptionTime;
        private Stopwatch mStopwatch;

        [TestInitialize]
        public void Initialize()
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
            using (var bitmap = new Bitmap(TestingConstants.NormalImage))
            {
                var encrypted = Encode(bitmap, new TextMessage(TestingConstants.NormalText));
                //ImageFunctions.WriteBitwiseToOutput(new LockBitmap(encrypted));
                var decrypted = Decode(encrypted);
                var result = decrypted.ConvertBack();
                Assert.IsTrue(result.StartsWith(TestingConstants.NormalText));
            }
        }

        [TestMethod]
        public void EncodeWithLsbIndicator()
        {
            using (var bitmap = new Bitmap(TestingConstants.NormalImage))
            {
                var encrypted = Encode(bitmap, new TextMessage(TestingConstants.NormalText), TestingConstants.Password.GetHashCode(), 4);

                var decrypted = Decode(encrypted, TestingConstants.Password.GetHashCode(), MessageType.Text, 4);

                var result = decrypted.ConvertBack().ToString();
                Assert.IsTrue(result.StartsWith(TestingConstants.NormalText));
            }
        }

        [TestMethod]
        public void EncodeImage()
        {
            using (var bitmap = new Bitmap(TestingConstants.NormalImage))
            {
                var encrypted = Encode(bitmap, new DocumentMessage(TestingConstants.SmallFlowers), TestingConstants.Password.GetHashCode());
                var decrypted = Decode(encrypted, TestingConstants.Password.GetHashCode(), MessageType.Document);
                var result = decrypted.ConvertBack();
                Assert.IsNotNull(result);
                Assert.IsTrue(File.Exists(result));
            }
        }

        [TestMethod]
        public void EncodeDocument()
        {
            using (var bitmap = new Bitmap(TestingConstants.NormalImage))
            {
                var encrypted = Encode(bitmap, new DocumentMessage(TestingConstants.Testdoc), TestingConstants.Password.GetHashCode());
                var decrypted = Decode(encrypted, TestingConstants.Password.GetHashCode(), MessageType.Document);
                var result = decrypted.ConvertBack();
                Assert.IsNotNull(result);
                Assert.IsTrue(File.Exists(result));
            }
        }

        [TestMethod]
        [ExpectedException(typeof (ContentLengthException))]
        public void EncodeWithoutSpace()
        {
            using (var bitmap = new Bitmap(TestingConstants.SmallKoala))
            {
                var encrypted = Encode(bitmap, new DocumentMessage(TestingConstants.SmallFlowers), TestingConstants.Password.GetHashCode());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EncodeWithoutText()
        {
            using (var bitmap = new Bitmap(TestingConstants.NormalImage))
            {
                var encrypted = Encode(bitmap, new TextMessage(string.Empty));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EncodeWithoutCover()
        {
            var encrypted = Encode(null, new TextMessage(TestingConstants.NormalText));
        }

        private ISecretMessage Decode(Bitmap encrypted)
        {
            mStopwatch.Start();
            var decrypted = Decode(encrypted, TestingConstants.Password.GetHashCode(), MessageType.Text);
            mStopwatch.Stop();
            mDecryptionTime = mStopwatch.Elapsed;
            Assert.IsFalse(decrypted == null);
            return decrypted;
        }

        private Bitmap Encode(Bitmap src, ISecretMessage value)
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

        protected abstract Bitmap Encode(Bitmap src, ISecretMessage value, int password, int additionalParam = 3);
        protected abstract ISecretMessage Decode(Bitmap src, int password, MessageType type, int additionalParam = 3);
    }
}