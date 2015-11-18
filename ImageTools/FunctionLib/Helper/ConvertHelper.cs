using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace FunctionLib.Helper
{
    public static class ConvertHelper
    {
        private static Encoding Encoder
        {
            get
            {
                return Encoding.GetEncoding(Constants.Encoding);
            }
        }

        public static byte[] ToByteArray(string value)
        {
            var result = Encoder.GetBytes(value);
            return result;
        }

        private static string ToString(byte[] value)
        {
            var result = Encoder.GetString(value);
            return result;
        }

        /// <summary>
        /// Converts a object into a byte-Array. Adds an Start and End-Tag to the object.
        /// </summary>
        /// <param name="value">The object can be a string or an path to an file.</param>
        /// <returns>byte[]</returns>
        public static byte[] ToByteArray(object value)
        {
            var str = value.ToString();
            if (value == null || string.IsNullOrEmpty(str))
            {
                throw new ArgumentException("value is null or empty.");
            }

            IEnumerable<byte> bytes;
            if (!File.Exists(str))
            {
                bytes = Constants.StartOfFileBytes("Text");
                bytes = bytes.Concat(ToByteArray(str));
            }
            else
            {
                var extension = Path.GetExtension(str).ToUpperInvariant();
                bytes = Constants.StartOfFileBytes(Constants.ImageExtensions.Contains(extension) ? "Image" : extension);
                using (var fileStream = File.Open(str, FileMode.Open))
                {
                    var compressedStream = MethodHelper.CompressStream(fileStream);
                    bytes = bytes.Concat(compressedStream);
                }
            }


            if (!bytes.Any())
            {
                throw new ArgumentException("Cant cast object to anything which contains byte[] for me, tho.");
            }
            bytes = bytes.Concat(Constants.EndOfFileBytes);
            return bytes.ToArray();
        }

        public static object ToObject(byte[] bytes)
        {
            string result;
            if (bytes == null || bytes.Length <= 0)
            {
                throw new ArgumentException("bytes is null or empty.");
            }
            var byteList = bytes.ToList();

            // Remove EndOfFile
            byteList.RemoveRange(byteList.Count - Constants.EndOfFileBytes.Length, Constants.EndOfFileBytes.Length);

            var index = byteList.IndexOf(ToByteArray(">").First());
            if (index == -1)
            {
                throw new ArgumentException("Start-Tag not found.");
            }

            var range = byteList.GetRange(0, index + 1);
            range.RemoveAt(0);
            range.RemoveAt(range.Count - 1);
            byteList.RemoveRange(0, index + 1);

            var type = ToString(range.ToArray()).ToUpperInvariant();
            switch (type)
            {
                case "TEXT":
                    result = ToString(byteList.ToArray());
                    break;
                case "IMAGE":
                    result = Constants.TempImagePath;
                    using (var uncompressedStream = MethodHelper.DecompressByteStream(byteList.ToArray()))
                    {
                        var returnImage = Image.FromStream(uncompressedStream);
                        returnImage.Save(result);                        
                    }
                    break;
                default:
                    if (!type.StartsWith("."))
                    {
                        throw new NotSupportedException("Invalid Start-Tag.");
                    }

                    using (var stream = MethodHelper.DecompressByteStream(byteList.ToArray()))
                    {
                        result = Constants.TempFilePath(type);
                        using (var fs = File.Create(result))
                        {
                            stream.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                    break;
            }
            return result;
        }
    }
}