using System;
using System.Drawing;
using FunctionLib.Helper;

namespace FunctionLib.Filter
{
    /*
    *   This code is based on the DIIT steganography project, which is available at the following address: http://diit.sourceforge.net/
    */

    public class Prewitt : Filter
    {
        public Prewitt(Bitmap image, int startbits, int endbits) : base(image, startbits, endbits)
        {
        }

        public Prewitt(LockBitmap image, int startbits, int endbits) : base(image, startbits, endbits)
        {
        }

        public override int GetValue(int x, int y)
        {
            // Gathering the matrices
            Color matrix1Left = default(Color),
                matrix1Leftup = default(Color),
                matrix1Leftdown = default(Color),
                matrix1Right = default(Color),
                matrix1Rightup = default(Color),
                matrix1Rightdown = default(Color),
                matrix2Up = default(Color),
                matrix2Upleft = default(Color),
                matrix2Upright = default(Color),
                matrix2Down = default(Color),
                matrix2Downleft = default(Color),
                matrix2Downright = default(Color);

            if (x == 0 || y == 0 || y == Image.Height - 1 || x == Image.Width - 1)
            {
                return 0;
            }

            if (x > 0)
            {
                matrix1Left = Image.GetPixel(x - 1, y);
                if (y > 0)
                {
                    matrix1Leftup = Image.GetPixel(x - 1, y - 1);
                    matrix2Upleft = Image.GetPixel(x - 1, y - 1);
                }
                if (y < Image.Height - 1)
                {
                    matrix1Leftdown = Image.GetPixel(x - 1, y + 1);
                    matrix2Downleft = Image.GetPixel(x - 1, y + 1);
                }
            }

            if (x < Image.Width - 1)
            {
                matrix1Right = Image.GetPixel(x + 1, y);
                if (y > 0)
                {
                    matrix1Rightup = Image.GetPixel(x + 1, y - 1);
                    matrix2Upright = Image.GetPixel(x + 1, y - 1);
                }
                if (y < Image.Height - 1)
                {
                    matrix1Rightdown = Image.GetPixel(x + 1, y + 1);
                    matrix2Downright = Image.GetPixel(x + 1, y + 1);
                }
            }

            if (y > 0)
            {
                matrix2Up = Image.GetPixel(x, y - 1);
            }

            if (y < Image.Height - 1)
            {
                matrix2Down = Image.GetPixel(x, y + 1);
            }

            // Calculating differences

            var redDiff = (int) Math.Sqrt(
                Math.Pow(
                    GetRed(matrix1Right) + GetRed(matrix1Rightup) + GetRed(matrix1Rightdown) -
                    (GetRed(matrix1Left) + GetRed(matrix1Leftup) + GetRed(matrix1Leftdown)), 2)
                +
                Math.Pow(
                    GetRed(matrix2Up) + GetRed(matrix2Upleft) + GetRed(matrix2Upright) -
                    (GetRed(matrix2Down) + GetRed(matrix2Downleft) + GetRed(matrix2Downright)), 2));

            var greenDiff = (int) Math.Sqrt(
                Math.Pow(
                    GetGreen(matrix1Right) + GetGreen(matrix1Rightup) + GetGreen(matrix1Rightdown) -
                    (GetGreen(matrix1Left) + GetGreen(matrix1Leftup) + GetGreen(matrix1Leftdown)), 2)
                +
                Math.Pow(
                    GetGreen(matrix2Up) + GetGreen(matrix2Upleft) + GetGreen(matrix2Upright) -
                    (GetGreen(matrix2Down) + GetGreen(matrix2Downleft) + GetGreen(matrix2Downright)), 2));

            var blueDiff = (int) Math.Sqrt(
                Math.Pow(
                    GetBlue(matrix1Right) + GetBlue(matrix1Rightup) + GetBlue(matrix1Rightdown) -
                    (GetBlue(matrix1Left) + GetBlue(matrix1Leftup) + GetBlue(matrix1Leftdown)), 2)
                +
                Math.Pow(
                    GetBlue(matrix2Up) + GetBlue(matrix2Upleft) + GetBlue(matrix2Upright) -
                    (GetBlue(matrix2Down) + GetBlue(matrix2Downleft) + GetBlue(matrix2Downright)), 2));

            // Returing the total result

            return Math.Abs(redDiff) + Math.Abs(greenDiff) + Math.Abs(blueDiff);
        }
    }
}