using System;
using System.Drawing;
using FunctionLib.Helper;

namespace FunctionLib.Steganalyse
{
    /*
    *   This code is based on the DIIT steganography project, which is available at the following address: http://diit.sourceforge.net/
    */
    public class PeakSignalToNoiseRatio : Analysis
    {
        protected override string Name
        {
            get { return "Peak Signal to Noise Ratio"; }
        }

        protected override double Calculation(LockBitmap originalBmp, LockBitmap steganoBmp)
        {
            Color orig;
            Color steg;
            var size = originalBmp.Height*originalBmp.Width;
            var totalDifference = 0.0;
            var maxDifference = 0.0;
            double currentDifference;
            for (var y = 0; y < originalBmp.Height; y++)
            {
                for (var x = 0; x < originalBmp.Width; x++)
                {
                    orig = originalBmp.GetPixel(x, y);
                    steg = steganoBmp.GetPixel(x, y);

                    currentDifference = Math.Pow(Math.Abs(orig.R) +
                                                 Math.Abs(orig.G) +
                                                 Math.Abs(orig.B), 2);
                    if (currentDifference >= maxDifference)
                        maxDifference = currentDifference;
                    totalDifference += Math.Pow(Math.Abs(orig.R
                                                         - steg.R)
                                                + Math.Abs(orig.G
                                                           - steg.G)
                                                + Math.Abs(orig.B
                                                           - steg.B), 2);
                }
            }
            return size*maxDifference/totalDifference;
        }
    }
}