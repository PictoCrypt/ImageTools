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
            //using (var output = new MemoryStream())
            //{
            //    using (var gzip = new GZipStream(output, CompressionMode.Compress))
            //    {
            //        using (src)
            //        {
            //            src.CopyTo(gzip);
            //        }
            //    }
            //    result = output.ToArray();
            //}

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
    }
}
