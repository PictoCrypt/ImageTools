using FunctionLib.Cryptography;
using FunctionLib.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.CryptTests
{
    [TestClass]
    public class RijndaelManagedTest : SymmetricAlgorithmBaseTestClass
    {
        public override string Encrypt(string value, string password)
        {
            return SymmetricAlgorithmBase.Encrypt(this, EncryptionMethod.Rijndael, value, password);
        }

        public override string Decrypt(string value, string password)
        {
            return SymmetricAlgorithmBase.Decrypt(this, EncryptionMethod.Rijndael, value, password);
        }
    }
}