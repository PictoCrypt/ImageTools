using System.IO;
using Ionic.Zip;

namespace FunctionLib.Helper
{
    public static class CompressionHelper
    {
        public static byte[] Compress(Stream src)
        {
            byte[] result;
            using (var compressed = new MemoryStream())
            {
                using (var zip = new ZipFile())
                {
                    zip.AddEntry("Object", src);
                    zip.Save(compressed);
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
                using (var zip = ZipFile.Read(ms))
                {
                    foreach (var entry in zip)
                    {
                        entry.Extract(result);
                    }
                }
            }
            return result;
        }
    }
}