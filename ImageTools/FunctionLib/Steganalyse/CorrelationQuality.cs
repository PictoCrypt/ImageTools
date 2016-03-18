using System;
using System.Drawing;
using FunctionLib.Helper;

namespace FunctionLib.Steganalyse
{
    public class CorrelationQuality : Analysis
    {
        protected override string Name
        {
            get { return "Correlation Quality"; }
        }

        protected override double Calculation(LockBitmap originalBmp, LockBitmap steganoBmp)
        {
            Color orig;
            Color steg;
            var totalDifference = 0.0;
            var originalDifference = 0.0;
            for (var y = 0; y < originalBmp.Height; y++)
            {
                for (var x = 0; x < originalBmp.Width; x++)
                {
                    orig = originalBmp.GetPixel(x, y);
                    steg = steganoBmp.GetPixel(x, y);
                    originalDifference += orig.R + orig.G + orig.B;
                    totalDifference += Math.Abs(orig.R*steg.R)
                                       + Math.Abs(orig.G*steg.G)
                                       + Math.Abs(orig.B*steg.B);
                }
            }
            return totalDifference/originalDifference;
        }
    }
}