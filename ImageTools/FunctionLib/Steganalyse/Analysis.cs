using System;
using System.Drawing;
using FunctionLib.Helper;

namespace FunctionLib.Steganalyse
{
    public abstract class Analysis
    {
        protected abstract string Name { get; }

        public double Calculate(Bitmap originalBmp, Bitmap steganoBmp)
        {
            if (originalBmp == null)
            {
                throw new ArgumentNullException(nameof(originalBmp));
            }

            if (originalBmp == null)
            {
                throw new ArgumentNullException(nameof(steganoBmp));
            }

            var original = new LockBitmap(originalBmp);
            original.LockBits();
            var stegano = new LockBitmap(steganoBmp);
            stegano.LockBits();
            var result = Calculation(original, stegano);

            original.UnlockBits();
            stegano.UnlockBits();
            return result;
        }

        protected abstract double Calculation(LockBitmap originalBmp, LockBitmap steganoBmp);
    }
}