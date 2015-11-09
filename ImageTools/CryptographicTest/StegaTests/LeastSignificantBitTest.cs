using System.Drawing;
using FunctionLib;
using FunctionLib.Steganography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CryptographicTest.StegaTests
{
    [TestClass]
    public class LeastSignificantBitTest : SteganographicAlogithmBaseTestClass
    {
        public override Bitmap Encrypt(Bitmap src, object value, int additionalParam)
        {
            return SteganographicAlgorithmBase.Encrypt(this, SteganographicMethod.LSB, src, value, additionalParam);
        }

        public override string Decrypt(Bitmap src, ResultingType type, int additionalParam)
        {
            return SteganographicAlgorithmBase.Decrypt(this, SteganographicMethod.LSB, src, type,
                additionalParam);
        }
    }
}