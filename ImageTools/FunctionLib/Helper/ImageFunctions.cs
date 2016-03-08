using System;
using System.Diagnostics;

namespace FunctionLib.Helper
{
    public static class ImageFunctions
    {
        public static void WriteBitwiseToOutput(LockBitmap src, int lsbIndicator = 3)
        {
            for (int y = 0; y < src.Height; y++)
            {
                for (int x = 0; x < src.Width; x++)
                {
                    var pixel = src.GetPixel(x, y);
                    //TODO implement lsb
                    var r = pixel.R & Convert.ToByte("111");
                    var g = pixel.G & Convert.ToByte("111");
                    var b = pixel.B & Convert.ToByte("111");
                    Debug.Write(string.Format("{0} {1} {2} \t", r, g, b));
                }
                Debug.WriteLine("");
            }
        }
    }
}