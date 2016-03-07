using System.Drawing;
using FunctionLib.Model.Message;
using FunctionLib.Steganography.Base;
using FunctionLib.Steganography.LSB;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.StegaTests
{
    [TestClass]
    public class LeastSignificantBitTest : SteganographicAlogithmBaseTestClass
    {
        public override Bitmap Encrypt(Bitmap src, ISecretMessage value, int password, int additionalParam)
        {
            return SteganographicAlgorithmBase.Encrypt(this, typeof (LeastSignificantBit), src, value, password,
                additionalParam);
        }

        public override ISecretMessage Decrypt(Bitmap src, int password, MessageType type, int additionalParam)
        {
            var result = SteganographicAlgorithmBase.Decrypt(this, typeof (LeastSignificantBit), src, password, type,
                additionalParam);
            return result;
        }
    }
}