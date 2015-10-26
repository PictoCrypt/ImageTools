using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CryptographicTest
{
    public abstract class SymmetricAlgorithmBaseTestClass : ISymmetricAlgorithmBaseTestClass
    {
        [TestMethod]
        public void NormalEncryptionTest()
        {
            var encrypted = Encrypt(Constants.Value, Constants.Password);
            Assert.IsFalse(string.IsNullOrEmpty(encrypted));

            var decrypted = Decrypt(encrypted, Constants.Password);
            Assert.IsFalse(string.IsNullOrEmpty(decrypted));

            Assert.IsTrue(Constants.Value.Equals(decrypted));
        }

        public abstract string Encrypt(string value, string password);

        public abstract string Decrypt(string value, string password);
    }
}