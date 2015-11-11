using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using FunctionLib.Enums;
using FunctionLib.Model;

namespace FunctionLib.Helper
{
    public static class ConvertHelper
    {
        public static byte[] ToByteArray(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentException("str is null or empty.");
            }
            if (File.Exists(str))
            {
                //return ZipFileHelper.ZipToBytes(str);
            }

            var encoder = Encoding.GetEncoding("ISO-8859-1");
            var result = encoder.GetBytes(str.ToCharArray());
            return result;
        }

        public static string ToString(byte[] bytes)
        {
            var encoder = Encoding.GetEncoding("ISO-8859-1");
            var result = encoder.GetString(bytes);
            return result;
        }

        public static byte[] ToByteArray(object value)
        {
            var str = value.ToString();
            if (value == null || string.IsNullOrEmpty(str))
            {
                throw new ArgumentException("value is null or empty.");
            }

            byte[] bytes;
            if (!File.Exists(str))
            {
                bytes = Constants.StartOfFileBytes("Text");
                var encoder = Encoding.GetEncoding("ISO-8859-1");
                bytes = bytes.Concat(encoder.GetBytes(str.ToCharArray())).ToArray();
            }
            else
            {
                var extension = Path.GetExtension(str);
                if (Constants.ImageExtensions.Contains(extension))
                {
                    using (var src = new Bitmap(str))
                    {
                        using (var stream = new MemoryStream())
                        {
                            bytes = Constants.StartOfFileBytes("Image");
                            src.Save(stream, src.RawFormat);
                            stream.Close();
                            bytes = bytes.Concat(stream.ToArray()).ToArray();
                        }
                    }
                }
                else
                {
                    using (var fileStream = File.Open(str, FileMode.Open))
                    {
                        using (var stream = new MemoryStream())
                        {
                            bytes = Constants.StartOfFileBytes(extension);
                            fileStream.CopyTo(stream);
                            bytes = bytes.Concat(stream.ToArray()).ToArray();
                        }
                    }
                }

                bytes = bytes.Concat(Constants.EndOfFileBytes).ToArray();
                return bytes;
            }


            using (var stream = new MemoryStream())
            {
                using (var fileStream = File.Open(str, FileMode.Open))
                {
                    fileStream.CopyTo(stream);
                    bytes = stream.ToArray();
                }
            }

            if (bytes.Length <= 0)
            {
            throw new ArgumentException("Cant cast object to anything which contains byte[] for me, tho.");

            }
            return bytes;
        }

        public static string ToImage(byte[] bytes)
        {
            var path = Constants.TempImagePath;

            using (var stream = new MemoryStream(bytes))
            {
                var returnImage = Image.FromStream(stream);
                returnImage.Save(path);
            }
            return path;
        }
    }
}