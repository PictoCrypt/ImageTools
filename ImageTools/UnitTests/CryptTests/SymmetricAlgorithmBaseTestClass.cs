using System;
using System.Diagnostics;
using FunctionLib.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.CryptTests
{
    public abstract class SymmetricAlgorithmBaseTestClass
    {
        private TimeSpan mDecryptionTime;
        private TimeSpan mEncryptionTime;
        //TODO: Falschtests

        private Stopwatch mStopwatch;

        protected abstract CryptographicAlgorithmImpl Algorithm { get; }

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
            var decrypted = Algorithm.Decode(encrypted, TestingConstants.Password);
            mStopwatch.Stop();
            mDecryptionTime = mStopwatch.Elapsed;
            Assert.IsFalse(string.IsNullOrEmpty(decrypted));
            return decrypted;
        }

        private string Encrypt(string value)
        {
            mStopwatch.Start();
            var encrypted = Algorithm.Encode(value, TestingConstants.Password);
            mStopwatch.Stop();
            mEncryptionTime = mStopwatch.Elapsed;
            mStopwatch.Reset();
            Assert.IsFalse(string.IsNullOrEmpty(encrypted));
            return encrypted;
        }
    }
}