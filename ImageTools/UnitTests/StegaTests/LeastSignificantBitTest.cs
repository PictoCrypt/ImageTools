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
        protected override Bitmap Encode(Bitmap src, ISecretMessage value, int password, int additionalParam)
        {
            return SteganographicAlgorithmBase.Encode(this, typeof (LeastSignificantBit), src, value, password,
                additionalParam);
        }

        protected override ISecretMessage Decode(Bitmap src, int password, MessageType type, int additionalParam)
        {
            var result = SteganographicAlgorithmBase.Decode(this, typeof (LeastSignificantBit), src, password, type,
                additionalParam);
            return result;
        }
    }
}