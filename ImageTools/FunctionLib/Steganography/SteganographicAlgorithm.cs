﻿using System;
using System.Collections.Generic;
using System.Drawing;
using FunctionLib.Helper;

namespace FunctionLib.Steganography
{
    public abstract class SteganographicAlgorithm : IDisposable
    {
        public SteganographicAlgorithm()
        {
            ChangedPixels = new List<Pixel>();
        }

        protected List<Pixel> ChangedPixels { get; }

        public void Dispose()
        {
            // GNDN
        }

        public abstract Bitmap Encrypt(Bitmap src, string value, int significantIndicator);
        public abstract string Decrypt(Bitmap src, int significantIndicator);
        public abstract string ChangeColor(string srcPath, Color color);
    }
}