using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
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

                var decrypted = Decode(encrypted);

                Assert.IsTrue(decrypted.ToString().StartsWith(TestingConstants.NormalText));

                WriteToOutput();
            }
        }

        [TestMethod]
        public void EncodeImage()
        {
            const int lsbIndicator = 4;

            using (var bitmap = new Bitmap(TestingConstants.NormalImage))
            {
                var encrypted = Encode(bitmap, new DocumentMessage(TestingConstants.SmallFlowers), TestingConstants.Password.GetHashCode(),
                    lsbIndicator);
                var decrypted = Decode(encrypted, TestingConstants.Password.GetHashCode(), MessageType.Text, lsbIndicator).ToString();

                Assert.IsNotNull(decrypted);
                Assert.IsTrue(File.Exists(decrypted));

                WriteToOutput();
            }
        }

        [TestMethod]
        public void EncodeDocument()
        {
            using (var bitmap = new Bitmap(TestingConstants.NormalImage))
            {
                var encrypted = Encode(bitmap, new DocumentMessage(TestingConstants.Testdoc), TestingConstants.Password.GetHashCode(),
                    TestingConstants.LsbIndicator);
                var decrypted =
                    Decode(encrypted, TestingConstants.Password.GetHashCode(), MessageType.Text, TestingConstants.LsbIndicator);

                Assert.IsFalse(decrypted.Bytes == null);
                Assert.IsTrue(File.Exists(decrypted.ConvertBack().ToString()));

                WriteToOutput();
            }
        }

        [TestMethod]
        [ExpectedException(typeof (TargetInvocationException))]
        public void EncodeWithoutSpace()
        {
            const int lsbIndicator = 4;
            using (var bitmap = new Bitmap(TestingConstants.SmallKoala))
            {
                var encrypted = Encode(bitmap, new DocumentMessage(TestingConstants.SmallFlowers), TestingConstants.Password.GetHashCode(),
                    lsbIndicator);
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

        private object Decode(Bitmap encrypted)
        {
            mStopwatch.Start();
            var decrypted = Decode(encrypted, TestingConstants.Password.GetHashCode(), MessageType.Text, TestingConstants.LsbIndicator);
            mStopwatch.Stop();
            mDecryptionTime = mStopwatch.Elapsed;
            Assert.IsFalse(decrypted == null);
            return decrypted;
        }

        private Bitmap Encode(Bitmap src, ISecretMessage value)
        {
            mStopwatch.Start();
            var encrypted = Encode(src, value, TestingConstants.Password.GetHashCode(), TestingConstants.LsbIndicator);
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

        protected abstract Bitmap Encode(Bitmap src, ISecretMessage value, int password, int additionalParam);
        protected abstract ISecretMessage Decode(Bitmap src, int password, MessageType type, int additionalParam);
    }
}