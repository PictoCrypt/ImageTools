using FunctionLib.Steganography;
using FunctionLib.Steganography.LSB;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.StegaTests
{
    [TestClass]
    public class LeastSignificantBitTest : SteganographicAlogithmBaseTestClass
    {
        protected override SteganographicAlgorithmImpl Algorithm { get {return new LeastSignificantBit();} }
    }
}