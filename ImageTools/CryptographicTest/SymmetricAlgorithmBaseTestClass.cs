using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CryptographicTest
{
    public abstract class SymmetricAlgorithmBaseTestClass : ISymmetricAlgorithmBaseTestClass
    {
        private Stopwatch mStopwatch;
        private TimeSpan mEncryptionTime;
        private TimeSpan mDecryptionTime;

        [TestInitialize]
        public void Initialize()
        {
            mStopwatch = new Stopwatch();
        }

        [TestMethod]
        public void NormalEncryptionTest()
        {
            var encrypted = Encrypt(Constants.NormalText);

            var decrypted = Decrypt(encrypted);

            Assert.IsTrue(Constants.NormalText.Equals(decrypted));

            WriteToOutput();
        }

        [TestMethod]
        public void LongTextEncryptionTest()
        {
            var encrypted = Encrypt(Constants.LongText);

            var decrypted = Decrypt(encrypted);

            Assert.IsTrue(Constants.LongText.Equals(decrypted));

            WriteToOutput();
        }

        private void WriteToOutput()
        {
            Trace.WriteLine(this.GetType() + "- TEST");
            Trace.WriteLine("Encryption-Time: " + mEncryptionTime);
            Trace.WriteLine("Decryption-Time: " + mDecryptionTime);
            Trace.WriteLine("");
            Trace.WriteLine("");
        }

        private string Decrypt(string encrypted)
        {
            mStopwatch.Start();
            var decrypted = Decrypt(encrypted, Constants.Password);
            mStopwatch.Stop();
            mDecryptionTime = mStopwatch.Elapsed;
            Assert.IsFalse(string.IsNullOrEmpty(decrypted));
            return decrypted;
        }

        private string Encrypt(string value)
        {
            mStopwatch.Start();
            var encrypted = Encrypt(value, Constants.Password);
            mStopwatch.Stop();
            mEncryptionTime = mStopwatch.Elapsed;
            mStopwatch.Reset();
            Assert.IsFalse(string.IsNullOrEmpty(encrypted));
            return encrypted;
        }

        public abstract string Encrypt(string value, string password);

        public abstract string Decrypt(string value, string password);
    }
}