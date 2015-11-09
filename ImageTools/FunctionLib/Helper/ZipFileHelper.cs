using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ionic.Zip;

namespace FunctionLib.Helper
{
    public static class ZipFileHelper
    {
        public static List<MemoryStream> OpenZip(string path)
        {
            var outputStreams = new List<MemoryStream>();
            using (var file = ZipFile.Read(path))
            {
                foreach (var entry in file)
                {
                    var ms = new MemoryStream();
                    entry.Extract(ms);
                    outputStreams.Add(ms);
                }
            }
            return outputStreams;
        }

        private static readonly List<byte> NullByteList = new List<byte>(Convert.ToByte(0)); 

        public static byte[] ZipToBytes(string path)
        {
            IEnumerable<byte> result = new List<byte>();
            var streams = OpenZip(path);
            foreach (var ms in streams)
            {
                using (ms)
                {
                    result = result.Concat(ms.ToArray());
                    result = result.Concat(NullByteList);
                }
            }
            return result.ToArray();
        }
    }
}