using FunctionLib.Cryptography;
using FunctionLib.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.CryptTests
{
    [TestClass]
    public class TwofishTest : SymmetricAlgorithmBaseTestClass
    {
        public override string Encrypt(string value, string password)
        {
            return SymmetricAlgorithmBase.Encrypt(this, EncryptionMethod.Twofish, value, password);
        }

        public override string Decrypt(string value, string password)
        {
            return SymmetricAlgorithmBase.Decrypt(this, EncryptionMethod.Twofish, value, password);
        }
    }
}