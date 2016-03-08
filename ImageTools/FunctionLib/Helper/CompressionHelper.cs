using System.IO;
using System.IO.Compression;

namespace FunctionLib.Helper
{
    public static class CompressionHelper
    {
        public static byte[] Compress(Stream src)
        {
            byte[] result;
            using (var compressed = new MemoryStream())
            {
                using (var gzip = new GZipStream(compressed, CompressionMode.Compress))
                {
                    src.CopyTo(gzip);
                }
                result = compressed.ToArray();
            }
            return result;
        }

        public static MemoryStream Decompress(byte[] bytes)
        {
            var result = new MemoryStream();
            using (var ms = new MemoryStream(bytes))
            {
                using (var gzip = new GZipStream(ms, CompressionMode.Decompress))
                {
                    gzip.CopyTo(result);
                }
            }
            return result;
        }
    }
}