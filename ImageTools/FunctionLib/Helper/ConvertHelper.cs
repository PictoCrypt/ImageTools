﻿using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace FunctionLib.Helper
{
    public static class ConvertHelper
    {
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

            if (bytes.Length <= 0)
            {
            throw new ArgumentException("Cant cast object to anything which contains byte[] for me, tho.");

            }
            return bytes;
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

            var index = byteList.IndexOf(Convert.ToByte(">"));
            if (index == -1)
            {
                throw new ArgumentException("Start-Tag not found.");
            }

            var range = byteList.GetRange(0, index);
            range.RemoveAt(0);
            range.RemoveAt(range.Count - 1);
            byteList.RemoveRange(0, index);

            var type = Convert.ToString(byteList.ToArray()).ToUpperInvariant();
            switch (type)
            {
                case "TEXT":
                    result = "";
                    break;
                case "IMAGE":
                    result = Constants.TempImagePath;

                    using (var stream = new MemoryStream(bytes))
                    {
                        var returnImage = Image.FromStream(stream);
                        returnImage.Save(result);
                    }
                    break;
                default:
                    if (!type.StartsWith("."))
                    {
                        throw new NotSupportedException("Invalid Start-Tag.");
                    }

                    using (var stream = new MemoryStream(byteList.ToArray()))
                    {
                        //TODO: TmpFilePath + extension
                        //result Change Extenstion
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

        //public static string ToImage(byte[] bytes)
        //{
        //    var path = Constants.TempImagePath;

        //    using (var stream = new MemoryStream(bytes))
        //    {
        //        var returnImage = Image.FromStream(stream);
        //        returnImage.Save(path);
        //    }
        //    return path;
        //}
    }
}