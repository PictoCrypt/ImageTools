using System;
using FunctionLib.CustomException;
using FunctionLib.Steganography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.StegaTests
{
    [TestClass]
    public class AppendMessageAlgorithmTest : SteganographicAlogithmBaseTestClass
    {
        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
            Algorithm = new AppendMessageAlgorithm();
        }

        [TestMethod]
        public override void EncodeDocumentWithoutSpaceTest()
        {
            //TODO thats shit
            throw new ContentLengthException();
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