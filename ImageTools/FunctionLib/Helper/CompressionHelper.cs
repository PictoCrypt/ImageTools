using System.IO;
using System.IO.Compression;

namespace FunctionLib.Helper
{
    public static class CompressionHelper
    {
        public static byte[] Compress(Stream src, CompressionLevel compression = CompressionLevel.NoCompression)
        {
            byte[] result;
            using (var compressed = new MemoryStream())
            {
                using (var gzip = new GZipStream(compressed, compression))
                {
                    using (src)
                    {
                        src.CopyTo(gzip);
                    }
                }
                result = compressed.ToArray();
            }
            return result;
        }

        public static MemoryStream Decompress(byte[] bytes, CompressionLevel compressed = CompressionLevel.NoCompression)
        {
            var result = new MemoryStream();
            using (var ms = new MemoryStream(bytes))
            {
                using (var gzip = new GZipStream(ms, compressed, true))
                {
                    gzip.CopyTo(result);
                }
            }
            return result;
        }
    }
}