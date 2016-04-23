using FunctionLib.Steganography;
using FunctionLib.Steganography.LSB;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.StegaTests
{
    [TestClass]
    public class FilterFirstTest : SteganographicAlogithmBaseTestClass
    {
        protected override SteganographicAlgorithmImpl Algorithm { get {return new FilterFirst();} }
    }
}