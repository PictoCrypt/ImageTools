using FunctionLib.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.CryptTests.Base
{
    public abstract class AsymmetricAlgorithmBaseTestClass : CryptAlgorithmTestClass
    {
        private string mPrivateKey;
        private string mPublicKey;

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
            var algorithm = Algorithm as CryptographicAssymetricAlgorithmImpl;
            if(algorithm != null)
            {
                var keys = algorithm.GenerateKeys();
                mPublicKey = keys[0];
                mPrivateKey = keys[1];
            }
        }

        protected override string Decrypt(string encrypted)
        {
            Stopwatch.Start();
            var decrypted = Algorithm.Decode(encrypted, mPrivateKey);
            Stopwatch.Stop();
            DecryptionTime = Stopwatch.Elapsed;
            Assert.IsFalse(string.IsNullOrEmpty(decrypted));
            return decrypted;
        }

        protected override string Encrypt(string value)
        {
            Stopwatch.Start();
            var encrypted = Algorithm.Encode(value, mPublicKey);
            Stopwatch.Stop();
            EncryptionTime = Stopwatch.Elapsed;
            Stopwatch.Reset();
            Assert.IsFalse(string.IsNullOrEmpty(encrypted));
            return encrypted;
        }
    }
}