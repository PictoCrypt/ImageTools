using System;
using System.Drawing;
using FunctionLib.Helper;

namespace FunctionLib.Filter
{
    /*
    *   This code is based on the DIIT steganography project, which is available at the following address: http://diit.sourceforge.net/
    */

    public class TraditionalLaplace : Filter
    {
        public TraditionalLaplace(Bitmap image, int startbits, int endbits) : base(image, startbits, endbits)
        {
        }

        public TraditionalLaplace(LockBitmap image, int startbits, int endbits) : base(image, startbits, endbits)
        {
        }

        public override int GetValue(int x, int y)
        {
            var pixel = Image.GetPixel(x, y);
            var left = default(Color);
            var up = default(Color);
            var right = default(Color);
            var down = default(Color);
            var pixelCount = 4;

            if (x <= 0)
            {
                pixelCount--;
            }
            else
            {
                left = Image.GetPixel(x - 1, y);
            }

            if (x >= Image.Width - 1)
            {
                pixelCount--;
            }
            else
            {
                right = Image.GetPixel(x + 1, y);
            }

            if (y <= 0)
            {
                pixelCount --;
            }
            else
            {
                up = Image.GetPixel(x, y - 1);
            }

            if (y >= Image.Height - 1)
            {
                pixelCount--;
            }
            else
            {
                down = Image.GetPixel(x, y + 1);
            }

            // Difference

            var result = pixelCount*GetLuminance(pixel) - GetLuminance(left) + GetLuminance(right) +
                         GetLuminance(up) + GetLuminance(down);
            return Convert.ToInt32(result);
        }

        private int GetLuminance(Color pixel)
        {
            //Y = 0.299R + 0.587G + 0.114B
            return (int) (0.299*GetRed(pixel) + 0.587*GetGreen(pixel)
                          + 0.114*GetBlue(pixel));
        }
    }
}