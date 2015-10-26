using System;
using FunctionLib.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CryptographicTest
{
    [TestClass]
    public class AesTest : SymmetricAlgorithmBaseTestClass
    {
        public override string Encrypt(string value, string password)
        {
            return SymmetricAlgorithmBase.Encrypt(value, password);
        }

        public override string Decrypt(string value, string password)
        {
            return SymmetricAlgorithmBase.Decrypt(value, password);
        }
    }
}
