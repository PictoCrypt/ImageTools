using System;
using System.Drawing;
using FunctionLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.FunctionLibTests
{
    [TestClass]
    public class HistogramTest
    {
        [TestMethod]
        public void GenerateHistogram()
        {
            var result = ImageFunctionLib.GettingHistogramData(new Bitmap(@"C:\Users\marius.schroeder\Desktop\Normal.png"), 3);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);

            var res2 = ImageFunctionLib.GettingHistogramData(new Bitmap(@"C:\Users\marius.schroeder\Desktop\Encrypted.png"), 3);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WrongByteArray()
        {
            ImageFunctionLib.GettingHistogramData(new Bitmap(TestingConstants.NormalImage), 3324);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ZeroByteArray()
        {
            ImageFunctionLib.GettingHistogramData(new Bitmap(TestingConstants.NormalImage), 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NullImage()
        {
            ImageFunctionLib.GettingHistogramData(null, 3);
        }
    }
}
