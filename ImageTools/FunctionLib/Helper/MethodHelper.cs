using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

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
            //var result = Encoding.UTF8.GetBytes(str.ToCharArray());
            return result;
        }

        public static string ByteArrayToString(byte[] arr)
        {
            var result = new UTF8Encoding();
            return result.GetString(arr);
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
                    //var color = pixel.ToArgb();
                    byteList.Add(pixel.R);
                    byteList.Add(pixel.G);
                    byteList.Add(pixel.B);
                }
                byteList.Add(Convert.ToByte(0));
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
        
        public static Bitmap ByteToBitmap(byte[] bytes)
        {
            var height = bytes.Count(x => x.Equals(byte.MinValue)) + 1;
            var width = Array.FindIndex(bytes, x => x.Equals(byte.MinValue));
            var tmp = GetTempImageStream(width, height);
            var result = new Bitmap(tmp);
            var lockBitmap = new LockBitmap(result);
            lockBitmap.LockBits();
            var row = 0;
            var column = 0;

            for (var i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] == byte.MinValue)
                {
                    row++;
                    column = 0;
                }

                var r = bytes[i++];
                var g = bytes[i++];
                var b = bytes[i++];

                var color = Color.FromArgb(r, g, b);
                lockBitmap.SetPixel(column, row, color);
                column++;
            }

            lockBitmap.UnlockBits();
            return result;
        }

        private static string GetTempImageStream(int width, int height)
        {
            var path = Path.GetTempPath() + Guid.NewGuid() + ".png";
            using (var bitmap = new Bitmap(width, height))
            {
                bitmap.Save(path);
            }
            return path;
        }
    }
}