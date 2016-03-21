using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using FunctionLib.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.FunctionLibTests
{
    [TestClass]
    public class AdditionalTests
    {
        [TestMethod]
        public void ImageFormatToFileFilterTest()
        {
            var result = ConvertHelper.GenerateFilter(new List<ImageFormat> {ImageFormat.Png, ImageFormat.Bmp});
            Assert.IsNotNull(result);
            Assert.AreEqual(result, "PNG|*.png|BMP|*.bmp");
        }
    }
}
