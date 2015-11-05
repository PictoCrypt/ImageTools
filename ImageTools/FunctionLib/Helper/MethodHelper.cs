using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
                return new byte[0];
            }
            var result = new ASCIIEncoding();
            return result.GetBytes(str);
        }

        public static string ByteArrayToString(byte[] arr)
        {
            var result = new ASCIIEncoding();
            return result.GetString(arr);
        }

        public static IEnumerable<byte> BitmaptoByteArray(Bitmap src)
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
            }
            lockBitmap.UnlockBits();
            return byteList;
        }
    }
}