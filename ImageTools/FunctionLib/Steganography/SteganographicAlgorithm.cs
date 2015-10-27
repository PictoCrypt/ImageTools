using System;
using System.Collections.Generic;
using System.Drawing;
using FunctionLib.Helper;

namespace FunctionLib.Steganography
{
    public abstract class SteganographicAlgorithm : IDisposable
    {
        protected List<Pixel> ChangedPixels { get; }

        public SteganographicAlgorithm()
        {
            ChangedPixels = new List<Pixel>();
        }

        public abstract Bitmap Encrypt(Bitmap src, string value);
        public abstract string Decrypt(Bitmap src);
        public abstract string ChangeColor(string srcPath, Color color);
        public void Dispose()
        {
            // GNDN
        }
    }
}