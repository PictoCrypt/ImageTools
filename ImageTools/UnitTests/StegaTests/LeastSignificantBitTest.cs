using System.Drawing;
using FunctionLib.Enums;
using FunctionLib.Steganography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.StegaTests
{
    [TestClass]
    public class LeastSignificantBitTest : SteganographicAlogithmBaseTestClass
    {
        public override Bitmap Encrypt(Bitmap src, string value, int additionalParam)
        {
            return SteganographicAlgorithmBase.Encrypt(this, SteganographicMethod.LSB, src, value, additionalParam);
        }

        public override object Decrypt(Bitmap src, int additionalParam)
        {
            var result =  SteganographicAlgorithmBase.Decrypt(this, SteganographicMethod.LSB, src, additionalParam);
            return result;
        }
    }
}