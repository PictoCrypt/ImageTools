using FunctionLib.Cryptography;
using FunctionLib.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.CryptTests
{
    [TestClass]
    public class Rc2Test : SymmetricAlgorithmBaseTestClass
    {
        public override string Encrypt(string value, string password)
        {
            return SymmetricAlgorithmBase.Encrypt(this, EncryptionMethod.RC2, value, password);
        }

        public override string Decrypt(string value, string password)
        {
            return SymmetricAlgorithmBase.Decrypt(this, EncryptionMethod.RC2, value, password);
        }
    }
}