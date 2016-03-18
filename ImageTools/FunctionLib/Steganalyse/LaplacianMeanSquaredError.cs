using System;
using FunctionLib.Filter;
using FunctionLib.Helper;

namespace FunctionLib.Steganalyse
{
    public class LaplacianMeanSquaredError : Analysis
    {
        protected override string Name
        {
            get { return "Laplacian Mean Squared Error"; }
        }

        protected override double Calculation(LockBitmap originalBmp, LockBitmap steganoBmp)
        {
            var origFiltered = new Laplace(originalBmp, 0, 8);
            var stegoFiltered = new Laplace(steganoBmp, 0, 8);
            var originalDiff = 0.0;
            var totalDiff = 0.0;

            for (var y = 0; y < originalBmp.Height; y++)
            {
                for (var x = 0; x < originalBmp.Width; x++)
                {
                    //TODO Shouldnt that be the other direction? First TOTAL and second ORIGINAL?
                    originalDiff += Math.Pow(origFiltered.GetValue(x, y) - stegoFiltered.GetValue(x, y), 2);
                    totalDiff += Math.Pow(origFiltered.GetValue(x, y), 2);
                }
            }

            return originalDiff/totalDiff;
        }
    }
}