using System.Security.Cryptography;
using FunctionLib.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.CryptTests
{
    [TestClass]
    public class DesTest : SymmetricAlgorithmBaseTestClass
    {
        public override string Encrypt(string value, string password)
        {
            return SymmetricAlgorithmBase.Encode(this, typeof (DESCryptoServiceProvider), value, password);
        }

        public override string Decrypt(string value, string password)
        {
            return SymmetricAlgorithmBase.Decode(this, typeof (DESCryptoServiceProvider), value, password);
        }
    }
}