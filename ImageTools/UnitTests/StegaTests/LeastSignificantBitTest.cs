using FunctionLib.Steganography.LSB;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.StegaTests
{
    [TestClass]
    public class LeastSignificantBitTest : SteganographicAlogithmBaseTestClass
    {
        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
            Algorithm = new FilterFirst();
        }
    }
}