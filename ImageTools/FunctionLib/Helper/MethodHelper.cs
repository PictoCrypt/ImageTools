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

        public static byte[] BitmaptoByteArray(Bitmap src)
        {
            var byteList = new List<byte>();
            var lockBitmap = new LockBitmap(src);
            lockBitmap.LockBits();
            for (var i = 0; i < lockBitmap.Height; i++)
            {
                for (var j = 0; j < lockBitmap.Width; j++)
                {
                    var pixel = lockBitmap.GetPixel(j, i);
                    byteList.Add(pixel.R);
                    byteList.Add(pixel.G);
                    byteList.Add(pixel.B);
                }
                if (lockBitmap.Height - 1 != i)
                {
                    byteList.Add(Convert.ToByte(0));
                    byteList.Add(Convert.ToByte(0));
                }
            }
            lockBitmap.UnlockBits();
            return byteList.ToArray();
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
            var width = ListHelper.IndexOf(bytes.ToList(), new List<byte> { byte.MinValue, byte.MinValue }) / 3;
            var height = (bytes.Length / 3) / width;

            var path = TempImagePath();
            using (var result = new Bitmap(width, height))
            {
                var lockBitmap = new LockBitmap(result);
                lockBitmap.LockBits();

                var index = 0;
                for (var y = 0; y < height; y++)
                {
                    for (var x = 0; x < width; x++)
                    {
                        var color = Color.FromArgb(bytes[index++], bytes[index++], bytes[index++]);

                        lockBitmap.SetPixel(x, y, color);
                    }
                    index = index + 2;
                }

                lockBitmap.UnlockBits();
                result.Save(path);
            }
            return path;
        }

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