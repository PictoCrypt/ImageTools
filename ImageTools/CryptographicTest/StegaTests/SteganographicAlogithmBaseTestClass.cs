using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using FunctionLib;
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
        public void EncryptImageTest()
        {
            var encrypted = Encrypt(Constants.SmallKoalaImage, Constants.SmallFlowersImage);

            var decrypted = Decrypt(encrypted, ResultingType.Image, Constants.NormalAdditionalParam);

            Assert.Equals(Constants.SmallFlowersImage, decrypted);

            WriteToOutput();
        }

        [TestMethod]
        [ExpectedException(typeof (TargetInvocationException))]
        public void EncryptionWithoutTextTest()
        {
            var encrypted = Encrypt(Constants.NormalBitmap, string.Empty);
        }

        private string Decrypt(Bitmap encrypted, ResultingType type = ResultingType.Text)
        {
            mStopwatch.Start();
            var decrypted = Decrypt(encrypted, type, Constants.NormalAdditionalParam);
            mStopwatch.Stop();
            mDecryptionTime = mStopwatch.Elapsed;
            Assert.IsFalse(string.IsNullOrEmpty(decrypted));
            return decrypted;
        }

        private Bitmap Encrypt(Bitmap src, object value)
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

        public abstract Bitmap Encrypt(Bitmap src, object value, int additionalParam);
        public abstract string Decrypt(Bitmap src, ResultingType type, int additionalParam);
    }
}