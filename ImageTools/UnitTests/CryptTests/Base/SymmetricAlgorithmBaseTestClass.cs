using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.CryptTests.Base
{
    public abstract class SymmetricAlgorithmBaseTestClass :  CryptAlgorithmTestClass
    {
        protected override string Decrypt(string encrypted)
        {
            Stopwatch.Start();
            var decrypted = Algorithm.Decode(encrypted, TestingConstants.Password);
            Stopwatch.Stop();
            DecryptionTime = Stopwatch.Elapsed;
            Assert.IsFalse(string.IsNullOrEmpty(decrypted));
            return decrypted;
        }

        protected override string Encrypt(string value)
        {
            Stopwatch.Start();
            var encrypted = Algorithm.Encode(value, TestingConstants.Password);
            Stopwatch.Stop();
            EncryptionTime = Stopwatch.Elapsed;
            Stopwatch.Reset();
            Assert.IsFalse(string.IsNullOrEmpty(encrypted));
            return encrypted;
        }
    }
}