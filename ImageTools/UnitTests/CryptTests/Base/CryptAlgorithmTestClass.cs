using System;
using System.Diagnostics;
using FunctionLib.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.CryptTests.Base
{
    public abstract class CryptAlgorithmTestClass
    {
        protected TimeSpan DecryptionTime;
        protected TimeSpan EncryptionTime;
        protected Stopwatch Stopwatch;
        protected abstract CryptographicAlgorithmImpl Algorithm { get; }
        protected abstract string Decrypt(string encrypted);
        protected abstract string Encrypt(string value);

        [TestMethod]
        public void NormalEncryptionTest()
        {
            var encrypted = Encrypt(TestingConstants.NormalText);

            var decrypted = Decrypt(encrypted);

            Assert.IsTrue(decrypted.StartsWith(TestingConstants.NormalText));

            WriteToOutput();
        }

        [TestMethod]
        public virtual void LongTextEncryptionTest()
        {
            var encrypted = Encrypt(TestingConstants.LongText);

            var decrypted = Decrypt(encrypted);

            Assert.IsTrue(decrypted.StartsWith(TestingConstants.LongText));

            WriteToOutput();
        }

        [TestInitialize]
        public virtual void Initialize()
        {
            Stopwatch = new Stopwatch();
        }

        private void WriteToOutput()
        {
            Trace.WriteLine(GetType() + "- TEST");
            Trace.WriteLine("Encryption-Time: " + EncryptionTime);
            Trace.WriteLine("Decryption-Time: " + DecryptionTime);
            Trace.WriteLine("");
            Trace.WriteLine("");
        }
    }
}