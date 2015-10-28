using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CryptographicTest
{
    public interface ISymmetricAlgorithmBaseTestClass
    {
        void NormalEncryptionTest();
        void LongTextEncryptionTest();
        string Encrypt(string value, string password);
        string Decrypt(string value, string password);
    }
}