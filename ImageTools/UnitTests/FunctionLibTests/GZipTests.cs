using System.Drawing;
using System.IO;
using FunctionLib.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.FunctionLibTests
{
    [TestClass]
    public class GZipTests
    {
        [TestMethod]
        // Dieser Test soll fehlschlagen, wenn GZip GRÖSSER als ein normaler Stream ist.
        public void GZipVersusNormalStreamTest()
        {
            byte[] msArrayBytes;
            using (var stream = new FileStream(TestingConstants.NormalImage, FileMode.Open))
            {
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    msArrayBytes = ms.ToArray();
                }
            }

            byte[] result;
            using (var stream = new FileStream(TestingConstants.NormalImage, FileMode.Open))
            {
                result = CompressionHelper.Compress(stream);
            }

            Assert.IsNotNull(msArrayBytes);
            Assert.IsTrue(msArrayBytes.Length > 0);
            Assert.IsTrue(result.Length < msArrayBytes.Length);
        }

        [TestMethod]
        public void GZipCompressTest()
        {
            var testBytes = ConvertHelper.StringToBytes(TestingConstants.NormalText);
            var src = new MemoryStream(testBytes);
            var result = CompressionHelper.Compress(src);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
        }

        [TestMethod]
        public void GZipImageCompressTest()
        {
            var stream = new FileStream(TestingConstants.NormalImage, FileMode.Open);
            var result = CompressionHelper.Compress(stream);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
        }

        [TestMethod]
        public void GZipDecompressTest()
        {
            var testBytes = ConvertHelper.StringToBytes(TestingConstants.NormalText);
            var src = new MemoryStream(testBytes);
            var result = CompressionHelper.Compress(src);
            var decompressed = CompressionHelper.Decompress(result);
            Assert.IsNotNull(decompressed);
            Assert.IsTrue(decompressed.Length > 0);
        }

        [TestMethod]
        public void GZipImageDecompressTest()
        {
            var stream = new FileStream(TestingConstants.NormalImage, FileMode.Open);
            var result = CompressionHelper.Compress(stream);
            var decompressed = CompressionHelper.Decompress(result);
            var img = Image.FromStream(decompressed);
            Assert.IsNotNull(img);
            Assert.IsFalse(img.Size.IsEmpty);
        }
    }
}