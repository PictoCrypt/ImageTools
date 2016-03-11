using System.Security.Cryptography;
using FunctionLib.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.CryptTests
{
    [TestClass]
    public class TripleDesTest : SymmetricAlgorithmBaseTestClass
    {
        public override string Encrypt(string value, string password)
        {
            return SymmetricAlgorithmBase.Encode(this, typeof (TripleDESCryptoServiceProvider), value, password);
        }

        public override string Decrypt(string value, string password)
        {
            return SymmetricAlgorithmBase.Decode(this, typeof (TripleDESCryptoServiceProvider), value, password);
        }
    }
}