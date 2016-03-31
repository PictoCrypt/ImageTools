using System;
using System.Drawing;
using FunctionLib.Helper;

namespace FunctionLib.Steganalyse
{
    /*
    *   This code is based on the DIIT steganography project, which is available at the following address: http://diit.sourceforge.net/
    */
    public class AverageAbsoluteDifference : Analysis
    {
        protected override string Name
        {
            get { return "Average Absolute Difference"; }
        }

        protected override double Calculation(LockBitmap originalBmp, LockBitmap steganoBmp)
        {
            var size = originalBmp.Height*originalBmp.Width;
            Color orig;
            Color steg;
            var difference = 0.0;
            for (var y = 0; y < originalBmp.Height; y++)
            {
                for (var x = 0; x < originalBmp.Width; x++)
                {
                    orig = originalBmp.GetPixel(x, y);
                    steg = steganoBmp.GetPixel(x, y);
                    difference += Math.Abs(orig.R - steg.R) + Math.Abs(orig.G - steg.G) +
                                  Math.Abs(orig.B - steg.B);
                }
            }
            return difference/size;
        }
    }
}