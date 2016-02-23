using System.Drawing;
using FunctionLib.Steganography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.StegaTests
{
    [TestClass]
    public class ComplexLeastSignificantBitTest : SteganographicAlogithmBaseTestClass
    {
        public override Bitmap Encrypt(Bitmap src, string value, int additionalParam)
        {
            return SteganographicAlgorithmBase.Encrypt(this, typeof (ComplexLeastSignificantBit), src, value,
                additionalParam);
        }

        public override object Decrypt(Bitmap src, int additionalParam)
        {
            return SteganographicAlgorithmBase.Decrypt(this, typeof (ComplexLeastSignificantBit), src, additionalParam);
        }
    }
}