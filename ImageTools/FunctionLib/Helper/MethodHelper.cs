using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace FunctionLib.Helper
{
    public static class MethodHelper
    {
        //private static bool CheckIfPossibleImage(int length)
        //{
        //    var result = length/3.0;
        //    var diff = Math.Abs(Math.Truncate(result) - result);
        //    return (diff < 0.0000001) || (diff > 0.9999999);
        //}

        public static int IndexOfWithinLastTwo(List<byte> byteList)
        {
            if (byteList.Count <= Constants.EndOfFileBytes.Length)
            {
                return -1;
            }

            var seq1 = byteList.GetRange(byteList.Count - Constants.EndOfFileBytes.Length, Constants.EndOfFileBytes.Length);
            var seq2 = byteList.GetRange(byteList.Count - Constants.EndOfFileBytes.Length - 1, Constants.EndOfFileBytes.Length);

            if (seq1.SequenceEqual(Constants.EndOfFileBytes))
            {
                return byteList.Count - Constants.EndOfFileBytes.Length;
            }
            if (seq2.SequenceEqual(Constants.EndOfFileBytes))
            {
                return byteList.Count - Constants.EndOfFileBytes.Length - 1;
            }
            return -1;
        }

        public static byte[] CompressStream(Stream src)
        {
            byte[] result;
            using (src)
            {
                using (var compressed = new MemoryStream())
                {
                    using (var gzip = new GZipStream(compressed, CompressionMode.Compress))
                    {
                        src.CopyTo(gzip);
                    }
                    result = compressed.ToArray();
                }
            }
            return result;
        }

        public static MemoryStream DecompressByteStream(byte[] bytes)
        {
            var result = new MemoryStream();
            using (var ms = new MemoryStream(bytes))
            {
                using (var gzip = new GZipStream(ms, CompressionMode.Decompress, true))
                {
                    gzip.CopyTo(result);
                }
            }
            return result;
        }
    }
}