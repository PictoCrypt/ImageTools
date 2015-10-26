using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CryptographicTest
{
    public interface ISymmetricAlgorithmBaseTestClass
    {
        [TestMethod]
        void NormalEncryptionTest();
        string Encrypt(string value, string password);
        string Decrypt(string value, string password);
    }
}