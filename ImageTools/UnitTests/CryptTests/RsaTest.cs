using FunctionLib.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.CryptTests.Base;

namespace UnitTests.CryptTests
{
    [TestClass]
    public class RsaTest : AsymmetricAlgorithmBaseTestClass
    {
        protected override CryptographicAlgorithmImpl Algorithm { get {return new RsaAlgorithm(); } }
    }
}