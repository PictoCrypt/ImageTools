using System;
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
        public static Random GetRandom(string seedText)
        {
            var seed = 0;
            seedText.Select(x => seed += x);
            var result = new Random(seed);
            return result;
        }

        public static int IndexOfWithinLastTwo(List<byte> byteList)
        {
            if (byteList.Count <= Constants.EndTag.Length)
            {
                return -1;
            }

            var seq1 = byteList.GetRange(byteList.Count - Constants.EndTag.Length, Constants.EndTag.Length);
            var seq2 = byteList.GetRange(byteList.Count - Constants.EndTag.Length - 1, Constants.EndTag.Length);

            if (seq1.SequenceEqual(Constants.EndTag))
            {
                return byteList.Count - Constants.EndTag.Length;
            }
            if (seq2.SequenceEqual(Constants.EndTag))
            {
                return byteList.Count - Constants.EndTag.Length - 1;
            }
            return -1;
        }

        public static byte[] CompressStream(Stream src)
        {
            byte[] result;
            using (var compressed = new MemoryStream())
            {
                using (var gzip = new GZipStream(compressed, CompressionMode.Compress))
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