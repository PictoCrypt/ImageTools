using System.Drawing;
using FunctionLib.Steganography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.StegaTests
{
    [TestClass]
    public class LeastSignificantBitTest : SteganographicAlogithmBaseTestClass
    {
        public override Bitmap Encrypt(Bitmap src, string value, int additionalParam)
        {
            return SteganographicAlgorithmBase.Encrypt(this, typeof(LeastSignificantBit), src, value, additionalParam);
        }

        public override object Decrypt(Bitmap src, int additionalParam)
        {
            var result = SteganographicAlgorithmBase.Decrypt(this, typeof(LeastSignificantBit), src, additionalParam);
            return result;
        }
    }
}