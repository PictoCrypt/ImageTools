using System;
using FunctionLib.CustomException;
using FunctionLib.Steganography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.StegaTests
{
    [TestClass]
    public class AppendMessageAlgorithmTest : SteganographicAlogithmBaseTestClass
    {

        [TestMethod]
        public override void EncodeDocumentWithoutSpaceTest()
        {
            //TODO thats shit
            throw new ContentLengthException();
        }

        protected override SteganographicAlgorithmImpl Algorithm
        {
            get
            {
                return new AppendMessageAlgorithm();
            }
        }

        [TestMethod]
        public override void EncodeTextWithoutSpaceTest()
        {
            //TODO thats shit
            throw new ContentLengthException();
        }

        [TestMethod]
        public override void EncodeWithWrongImageFormatTest()
        {
            //TODO thats shit
            throw new BadImageFormatException();
        }
    }
}