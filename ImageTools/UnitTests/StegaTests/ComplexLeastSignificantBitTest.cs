using System.Drawing;
using FunctionLib.Model.Message;
using FunctionLib.Steganography;
using FunctionLib.Steganography.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.StegaTests
{
    [TestClass]
    public class ComplexLeastSignificantBitTest : SteganographicAlogithmBaseTestClass
    {
        public override Bitmap Encrypt(Bitmap src, ISecretMessage value, int password, int additionalParam)
        {
            return SteganographicAlgorithmBase.Encrypt(this, typeof(ComplexLeastSignificantBit), src, value, password,
                additionalParam);
        }

        public override ISecretMessage Decrypt(Bitmap src, int password, MessageType type, int additionalParam)
        {
            return SteganographicAlgorithmBase.Decrypt(this, typeof (ComplexLeastSignificantBit), src, password, type,
                additionalParam);
        }
    }
}