using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CryptographicTest.StegaTests
{
    public abstract class SteganographicAlogithmBaseTestClass
    {
        private TimeSpan mDecryptionTime;
        private TimeSpan mEncryptionTime;
        //TODO: Falschtests

        private Stopwatch mStopwatch;

        [TestInitialize]
        public void Initialize()
        {
            mStopwatch = new Stopwatch();
        }

        [TestMethod]
        public void NormalEncryptionTest()
        {
            var encrypted = Encrypt(Constants.NormalBitmap, Constants.NormalText);

            var decrypted = Decrypt(encrypted);

            Assert.IsTrue(decrypted.StartsWith(Constants.NormalText));

            WriteToOutput();
        }

        [TestMethod]
        [ExpectedException(typeof(TargetInvocationException))]
        public void EncryptionWithoutTextTest()
        {
            var encrypted = Encrypt(Constants.NormalBitmap, string.Empty);
        }

        private string Decrypt(Bitmap encrypted)
        {
            mStopwatch.Start();
            var decrypted = Decrypt(encrypted, Constants.NormalAdditionalParam);
            mStopwatch.Stop();
            mDecryptionTime = mStopwatch.Elapsed;
            Assert.IsFalse(string.IsNullOrEmpty(decrypted));
            return decrypted;
        }

        private Bitmap Encrypt(Bitmap src, string value)
        {
            mStopwatch.Start();
            var encrypted = Encrypt(src, value, Constants.NormalAdditionalParam);
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

        public abstract Bitmap Encrypt(Bitmap src, string value, int additionalParam);
        public abstract string Decrypt(Bitmap src, int additionalParam);
    }
}