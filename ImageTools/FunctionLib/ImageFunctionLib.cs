using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace FunctionLib
{
    public static class ImageFunctionLib
    {
        public static Bitmap Diff(Bitmap image1, Bitmap image2, int x1, int y1, int x2, int y2, int width, int height)
        {
            var diffBm = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    //Get Both Colours at the pixel point
                    var col1 = image1.GetPixel(x1 + x, y1 + y);
                    var col2 = image2.GetPixel(x2 + x, y2 + y);

                    //Get the difference RGB
                    var r = Math.Abs(col1.R - col2.R);
                    var g = Math.Abs(col1.G - col2.G);
                    var b = Math.Abs(col1.B - col2.B);

                    //Invert the difference average
                    var dif = 255 - ((r + g + b)/3);

                    //Create new grayscale rgb colour
                    var newcol = Color.FromArgb(dif, dif, dif);

                    diffBm.SetPixel(x, y, newcol);
                }
            }
            return diffBm;
        }

        public static Bitmap ChangeColor(Bitmap src, Color color)
        {
            var result = new Bitmap(src);
            var lockBitmap = new LockBitmap(result);
            lockBitmap.LockBits();

            var compareClr = Color.FromArgb(255, 255, 255, 255);
            for (var y = 0; y < lockBitmap.Height; y++)
            {
                for (var x = 0; x < lockBitmap.Width; x++)
                {
                    if (lockBitmap.GetPixel(x, y) == compareClr)
                    {
                        lockBitmap.SetPixel(x, y, color);
                    }
                }
            }
            lockBitmap.UnlockBits();
            return result;
        }
    }
}