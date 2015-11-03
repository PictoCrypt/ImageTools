using System.Drawing;
using FunctionLib;
using FunctionLib.Steganography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CryptographicTest.StegaTests
{
    [TestClass]
    public class LeastSignificantBitTestByMarius : SteganographicAlogithmBaseTestClass
    {
        public override Bitmap Encrypt(Bitmap src, string value, int additionalParam)
        {
            return SteganographicAlgorithmBase.Encrypt(this, SteganographicMethod.TestMethod, src, value, additionalParam);
        }

        public override string Decrypt(Bitmap src, int additionalParam)
        {
            return SteganographicAlgorithmBase.Decrypt(this, SteganographicMethod.TestMethod, src, additionalParam);
        }
    }
}