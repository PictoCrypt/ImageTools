using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CryptographicTest.CryptTests
{
    public abstract class SymmetricAlgorithmBaseTestClass : ISymmetricAlgorithmBaseTestClass
    {
        private TimeSpan mDecryptionTime;
        private TimeSpan mEncryptionTime;
        //TODO: Falschtests

        private Stopwatch mStopwatch;

        [TestMethod]
        public void NormalEncryptionTest()
        {
            var encrypted = Encrypt(TestingConstants.NormalText);

            var decrypted = Decrypt(encrypted);

            Assert.IsTrue(TestingConstants.NormalText.Equals(decrypted));

            WriteToOutput();
        }

        [TestMethod]
        public void LongTextEncryptionTest()
        {
            var encrypted = Encrypt(TestingConstants.LongText);

            var decrypted = Decrypt(encrypted);

            Assert.IsTrue(TestingConstants.LongText.Equals(decrypted));

            WriteToOutput();
        }

        public abstract string Encrypt(string value, string password);

        public abstract string Decrypt(string value, string password);

        [TestInitialize]
        public void Initialize()
        {
            mStopwatch = new Stopwatch();
        }

        private void WriteToOutput()
        {
            Trace.WriteLine(GetType() + "- TEST");
            Trace.WriteLine("Encryption-Time: " + mEncryptionTime);
            Trace.WriteLine("Decryption-Time: " + mDecryptionTime);
            Trace.WriteLine("");
            Trace.WriteLine("");
        }

        private string Decrypt(string encrypted)
        {
            mStopwatch.Start();
            var decrypted = Decrypt(encrypted, TestingConstants.Password);
            mStopwatch.Stop();
            mDecryptionTime = mStopwatch.Elapsed;
            Assert.IsFalse(string.IsNullOrEmpty(decrypted));
            return decrypted;
        }

        private string Encrypt(string value)
        {
            mStopwatch.Start();
            var encrypted = Encrypt(value, TestingConstants.Password);
            mStopwatch.Stop();
            mEncryptionTime = mStopwatch.Elapsed;
            mStopwatch.Reset();
            Assert.IsFalse(string.IsNullOrEmpty(encrypted));
            return encrypted;
        }
    }
}