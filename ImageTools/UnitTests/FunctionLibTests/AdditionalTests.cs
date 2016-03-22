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
            var result = ConvertHelper.GenerateFilter(new List<ImageFormat> {ImageFormat.Png, ImageFormat.Bmp, ImageFormat.Jpeg});
            Assert.IsNotNull(result);
            Assert.AreEqual(result, "All Formats|*.png;*.bmp;*.jpg|PNG|*.png|BMP|*.bmp|JPEG|*.jpg");
        }
    }
}
