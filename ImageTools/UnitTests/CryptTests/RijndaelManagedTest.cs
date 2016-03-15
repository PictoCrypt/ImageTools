using FunctionLib.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.CryptTests
{
    [TestClass]
    public class RijndaelManagedTest : SymmetricAlgorithmBaseTestClass
    {
        protected override CryptographicAlgorithmImpl Algorithm
        {
            get { return new RijndaelAlgorithm(); }
        }
    }
}