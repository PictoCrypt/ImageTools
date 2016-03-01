using System.Drawing;
using FunctionLib.Steganography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.StegaTests
{
    [TestClass]
    public class ComplexLeastSignificantBitTest : SteganographicAlogithmBaseTestClass
    {
        public override Bitmap Encrypt(Bitmap src, string value, string password, int additionalParam)
        {
            return SteganographicAlgorithmBase.Encrypt(this, typeof (ComplexLeastSignificantBit), src, value, password,
                additionalParam);
        }

        public override object Decrypt(Bitmap src, string password, int additionalParam)
        {
            return SteganographicAlgorithmBase.Decrypt(this, typeof (ComplexLeastSignificantBit), src, password, additionalParam);
        }
    }
}