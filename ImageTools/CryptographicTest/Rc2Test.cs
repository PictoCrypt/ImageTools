using System.Security.Cryptography;
using FunctionLib.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CryptographicTest
{
    [TestClass]
    public class Rc2Test : SymmetricAlgorithmBaseTestClass
    {
        public override string Encrypt(string value, string password)
        {
            return SymmetricAlgorithmBase.Encrypt<RC2CryptoServiceProvider>(value, password);
        }

        public override string Decrypt(string value, string password)
        {
            return SymmetricAlgorithmBase.Decrypt<RC2CryptoServiceProvider>(value, password);
        }
    }
}