using System.Drawing;
using FunctionLib.Steganography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.StegaTests
{
    [TestClass]
    public class LeastSignificantBitTest : SteganographicAlogithmBaseTestClass
    {
        public override Bitmap Encrypt(Bitmap src, string value, int password, int additionalParam)
        {
            return SteganographicAlgorithmBase.Encrypt(this, typeof (LeastSignificantBit), src, value, password,
                additionalParam);
        }

        public override object Decrypt(Bitmap src, int password, int additionalParam)
        {
            var result = SteganographicAlgorithmBase.Decrypt(this, typeof (LeastSignificantBit), src, password,
                additionalParam);
            return result;
        }
    }
}