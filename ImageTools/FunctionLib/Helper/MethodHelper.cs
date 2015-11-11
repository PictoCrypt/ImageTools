using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using FunctionLib.Model;

namespace FunctionLib.Helper
{
    public static class MethodHelper
    {
        public static string ExecutiongPath
        {
            get
            {
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                return path;
            }
        }

        public static byte[] StringToByteArray(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentException("str is null or empty.");
            }
            if (File.Exists(str))
            {
                return ZipFileHelper.ZipToBytes(str);
            }

            var encoder = Encoding.GetEncoding("ISO-8859-1");
            var result = encoder.GetBytes(str.ToCharArray());
            return result;
        }

        public static string ByteArrayToString(byte[] bytes)
        {
            var result = new UTF8Encoding();
            return result.GetString(bytes);
        }

        private static byte[] BitmaptoByteArray(Bitmap src)
        {
            byte[] bytes;
            using (var stream = new MemoryStream())
            {
                src.Save(stream, src.RawFormat);
                stream.Close();
                bytes = stream.ToArray();
            }
            return bytes;
        }

        public static byte[] ToByteArray(object value)
        {
            if (value == null)
            {
                throw new ArgumentException("value is null.");
            }

            var str = value as string;
            if (str != null)
            {
                return StringToByteArray(str);
            }
            var bmp = value as Bitmap;
            if (bmp != null)
            {
                return BitmaptoByteArray(bmp);
            }
            throw new NotImplementedException("Cant cast object to anything which contains byte[] for me, tho.");
        }
        
        public static string ByteToBitmap(byte[] bytes)
        {
            var path = TempImagePath();

            using (var stream = new MemoryStream(bytes))
            {
                var returnImage = Image.FromStream(stream);
                returnImage.Save(path);
            }
            return path;
        }

        //private static bool CheckIfPossibleImage(int length)
        //{
        //    var result = length/3.0;
        //    var diff = Math.Abs(Math.Truncate(result) - result);
        //    return (diff < 0.0000001) || (diff > 0.9999999);
        //}

        private static string TempImagePath()
        {
            return Path.GetTempPath() + Guid.NewGuid() + ".png";
        }

        private static string GetTempImageStream(string path, int width, int height)
        {
            using (var bitmap = new Bitmap(width, height))
            {
                bitmap.Save(path);
            }
            return path;
        }
    }
}