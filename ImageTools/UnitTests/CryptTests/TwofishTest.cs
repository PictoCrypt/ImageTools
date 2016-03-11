using FunctionLib.Cryptography;
using FunctionLib.Cryptography.Twofish;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.CryptTests
{
    [TestClass]
    public class TwofishTest : SymmetricAlgorithmBaseTestClass
    {
        public override string Encrypt(string value, string password)
        {
            return SymmetricAlgorithmBase.Encode(this, typeof (Twofish), value, password);
        }

        public override string Decrypt(string value, string password)
        {
            return SymmetricAlgorithmBase.Decode(this, typeof (Twofish), value, password);
        }
    }
}