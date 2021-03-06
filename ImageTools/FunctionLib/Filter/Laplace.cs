﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FunctionLib.Helper;

namespace FunctionLib.Filter
{
    /*
    *   This code is based on the DIIT steganography project, which is available at the following address: http://diit.sourceforge.net/
    */

    public class Laplace : Filter
    {
        public Laplace(Bitmap image, int startbits, int endbits) : base(image, startbits, endbits)
        {
        }

        public Laplace(LockBitmap image, int startbits, int endbits) : base(image, startbits, endbits)
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
                pixelCount--;
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

            //work out the colours individually
            var diff = CalculateDifference(pixel, left, right, up, down, pixelCount);

            //return the results...
            return diff.Sum(Math.Abs);
        }

        private IEnumerable<int> CalculateDifference(Color pixel, Color left, Color right, Color up, Color down,
            int pixelCount)
        {
            var result = new int[3];
            result[0] = GetRed(pixel)*pixelCount - (GetRed(left) + GetRed(right) + GetRed(up) + GetRed(down));
            result[1] = GetGreen(pixel)*pixelCount - (GetGreen(left) + GetGreen(right) + GetGreen(up) + GetGreen(down));
            result[1] = GetBlue(pixel)*pixelCount - (GetBlue(left) + GetBlue(right) + GetBlue(up) + GetBlue(down));
            return result;
        }
    }
}