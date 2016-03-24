using FunctionLib.Steganography;
using FunctionLib.Steganography.LSB;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.StegaTests
{
    [TestClass]
    public class ExtendedLsbTest : SteganographicAlogithmBaseTestClass
    {
        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
            Algorithm = new ExtendedLsb();
        }
    }
}