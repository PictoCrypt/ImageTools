using System.Drawing;
using FunctionLib.Enums;
using FunctionLib.Steganography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.StegaTests
{
    [TestClass]
    public class ComplexLeastSignificantBitTest : SteganographicAlogithmBaseTestClass
    {
        public override Bitmap Encrypt(Bitmap src, string value, int additionalParam)
        {
            return SteganographicAlgorithmBase.Encrypt(this, SteganographicMethod.ComplexLSB, src, value,
                additionalParam);
        }
        
        public override object Decrypt(Bitmap src, int additionalParam)
        {
            return SteganographicAlgorithmBase.Decrypt(this, SteganographicMethod.ComplexLSB, src, additionalParam);
        }
    }
}