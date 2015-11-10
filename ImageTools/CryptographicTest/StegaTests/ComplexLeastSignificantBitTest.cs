using System.Drawing;
using FunctionLib;
using FunctionLib.Steganography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CryptographicTest.StegaTests
{
    [TestClass]
    public class ComplexLeastSignificantBitTest : SteganographicAlogithmBaseTestClass
    {
        public override Bitmap Encrypt(Bitmap src, object value, int additionalParam)
        {
            return SteganographicAlgorithmBase.Encrypt(this, SteganographicMethod.ComplexLSB, src, value,
                additionalParam);
        }

        public override object Decrypt(Bitmap src, ResultingType type, int additionalParam)
        {
            return SteganographicAlgorithmBase.Decrypt(this, SteganographicMethod.ComplexLSB, src, ResultingType.Text,
                additionalParam);
        }
    }
}