using System;
using System.Drawing;
using FunctionLib.Helper;

namespace FunctionLib.Steganalyse
{
    /*
    *   This code is based on the DIIT steganography project, which is available at the following address: http://diit.sourceforge.net/
    */
    public class MeanSquaredError : Analysis
    {
        protected override string Name
        {
            get { return "Mean Squared Error"; }
        }

        protected override double Calculation(LockBitmap originalBmp, LockBitmap steganoBmp)
        {
            var size = originalBmp.Height*originalBmp.Width;
            var totalDifference = 0.0;
            Color orig;
            Color steg;
            for (var y = 0; y < originalBmp.Height; y++)
            {
                for (var x = 0; x < originalBmp.Width; x++)
                {
                    orig = originalBmp.GetPixel(x, y);
                    steg = steganoBmp.GetPixel(x, y);
                    totalDifference += Math.Pow(Math.Abs(orig.R - steg.R)
                                                + Math.Abs(orig.G - steg.G)
                                                + Math.Abs(orig.B - steg.B), 2);
                }
            }

            return totalDifference/size;
        }
    }
}