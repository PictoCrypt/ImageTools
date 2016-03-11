using System.Security.Cryptography;
using FunctionLib.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.CryptTests
{
    [TestClass]
    public class AesTest : SymmetricAlgorithmBaseTestClass
    {
        public override string Encrypt(string value, string password)
        {
            return SymmetricAlgorithmBase.Encode(this, typeof (AesCryptoServiceProvider), value, password);
        }

        public override string Decrypt(string value, string password)
        {
            return SymmetricAlgorithmBase.Decode(this, typeof (AesCryptoServiceProvider), value, password);
        }
    }
}