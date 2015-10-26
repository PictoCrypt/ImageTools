using System;
using FunctionLib.Cryptography;
using FunctionLib.Cryptography.Twofish;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CryptographicTest
{
    [TestClass]
    public class TwofishTest : SymmetricAlgorithmBaseTestClass
    {
        public override string Encrypt(string value, string password)
        {
            return SymmetricAlgorithmBase.Encrypt<Twofish>(value, password);
        }

        public override string Decrypt(string value, string password)
        {
            return SymmetricAlgorithmBase.Decrypt<Twofish>(value, password);
        }
    }
}