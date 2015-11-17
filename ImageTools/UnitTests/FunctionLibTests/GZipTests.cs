using System.Drawing;
using System.IO;
using System.IO.Compression;
using FunctionLib.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.FunctionLibTests
{
    [TestClass]
    public class GZipTests
    {
        [TestMethod]
        public void GZipCompressTest()
        {
            var testBytes = ConvertHelper.ToByteArray(TestingConstants.NormalText);
            var src = new MemoryStream(testBytes);
            var result = MethodHelper.CompressStream(src);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
        }

        [TestMethod]
        public void GZipImageCompressTest()
        {
            var stream = new FileStream(TestingConstants.NormalImage, FileMode.Open);
            var result = MethodHelper.CompressStream(stream);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
        }

        [TestMethod]
        public void GZipDecompressTest()
        {
            var testBytes = ConvertHelper.ToByteArray(TestingConstants.NormalText);
            var src = new MemoryStream(testBytes);
            var result = MethodHelper.CompressStream(src);
            var decompressed = MethodHelper.DecompressByteStream(result);
            Assert.IsNotNull(decompressed);
            Assert.IsTrue(decompressed.Length > 0);
        }

        [TestMethod]
        public void GZipImageDecompressTest()
        {
            var stream = new FileStream(TestingConstants.NormalImage, FileMode.Open);
            var result = MethodHelper.CompressStream(stream);
            var decompressed = MethodHelper.DecompressByteStream(result);
            var img = Image.FromStream(decompressed);
            Assert.IsNotNull(img);
            Assert.IsFalse(img.Size.IsEmpty);
        }
    }
}
