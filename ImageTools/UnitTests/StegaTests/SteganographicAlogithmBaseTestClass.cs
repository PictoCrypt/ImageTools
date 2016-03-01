using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
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

        [TestMethod]
        public void NormalEncryptionTest()
        {
            using (var bitmap = new Bitmap(TestingConstants.NormalImage))
            {
                var encrypted = Encrypt(bitmap, TestingConstants.NormalText);

                var decrypted = Decrypt(encrypted);

                Assert.IsTrue(decrypted.ToString().StartsWith(TestingConstants.NormalText));

                WriteToOutput();
            }
        }

        [TestMethod]
        //[ExpectedException(typeof(ArgumentOutOfRangeException))]
        [ExpectedException(typeof (TargetInvocationException))]
        public void EncryptImageWithNotEnoughSpace()
        {
            const int lsbIndicator = 4;
            using (var bitmap = new Bitmap(TestingConstants.SmallKoala))
            {
                var encrypted = Encrypt(bitmap, TestingConstants.SmallFlowers, TestingConstants.Password, lsbIndicator);
            }
        }

        [TestMethod]
        public void EncryptImageTest()
        {
            const int lsbIndicator = 4;

            using (var bitmap = new Bitmap(TestingConstants.NormalImage))
            {
                var encrypted = Encrypt(bitmap, TestingConstants.SmallFlowers, TestingConstants.Password, lsbIndicator);
                var decrypted = Decrypt(encrypted, TestingConstants.Password, lsbIndicator).ToString();

                Assert.IsNotNull(decrypted);
                Assert.IsTrue(File.Exists(decrypted));

                WriteToOutput();
            }
        }

        [TestMethod]
        public void EncryptDocTest()
        {
            using (var bitmap = new Bitmap(TestingConstants.NormalImage))
            {
                var encrypted = Encrypt(bitmap, TestingConstants.Testdoc, TestingConstants.Password,
                    TestingConstants.LsbIndicator);
                var decrypted = Decrypt(encrypted, TestingConstants.Password, TestingConstants.LsbIndicator) as string;

                Assert.IsFalse(string.IsNullOrEmpty(decrypted));
                Assert.IsTrue(File.Exists(decrypted));

                WriteToOutput();
            }
        }

        [TestMethod]
        [ExpectedException(typeof (TargetInvocationException))]
        public void EncryptionWithoutTextTest()
        {
            using (var bitmap = new Bitmap(TestingConstants.NormalImage))
            {
                var encrypted = Encrypt(bitmap, string.Empty);
            }
        }

        private object Decrypt(Bitmap encrypted)
        {
            mStopwatch.Start();
            var decrypted = Decrypt(encrypted, TestingConstants.Password, TestingConstants.LsbIndicator);
            mStopwatch.Stop();
            mDecryptionTime = mStopwatch.Elapsed;
            Assert.IsFalse(decrypted == null);
            return decrypted;
        }

        private Bitmap Encrypt(Bitmap src, string value)
        {
            mStopwatch.Start();
            var encrypted = Encrypt(src, value, TestingConstants.Password, TestingConstants.LsbIndicator);
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

        public abstract Bitmap Encrypt(Bitmap src, string value, string password, int additionalParam);
        public abstract object Decrypt(Bitmap src, string password, int additionalParam);
    }
}