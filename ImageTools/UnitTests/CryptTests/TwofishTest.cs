using FunctionLib.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.CryptTests.Base;

namespace UnitTests.CryptTests
{
    [TestClass]
    public class TwofishTest : SymmetricAlgorithmBaseTestClass
    {
        protected override CryptographicAlgorithmImpl Algorithm
        {
            get { return new TwofishAlgorithm(); }
        }
    }
}