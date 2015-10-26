using System.Security.Cryptography;
using FunctionLib.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CryptographicTest
{
    [TestClass]
    public class DesTest : SymmetricAlgorithmBaseTestClass
    {
        public override string Encrypt(string value, string password)
        {
            return SymmetricAlgorithmBase.Encrypt<DESCryptoServiceProvider>(value, password);
        }

        public override string Decrypt(string value, string password)
        {
            return SymmetricAlgorithmBase.Decrypt<DESCryptoServiceProvider>(value, password);
        }
    }
}