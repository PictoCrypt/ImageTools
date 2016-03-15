using FunctionLib.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.CryptTests
{
    [TestClass]
    public class DesTest : SymmetricAlgorithmBaseTestClass
    {
        protected override CryptographicAlgorithmImpl Algorithm
        {
            get { return new DesAlgorithm(); }
        }
    }
}