using System;
using System.Drawing;
using FunctionLib.Helper;

namespace FunctionLib.Filter
{
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
            Color matrix1Left = default(Color), matrix1Leftup = default(Color), matrix1Leftdown = default(Color),
                matrix1Right = default(Color), matrix1Rightup = default(Color), matrix1Rightdown = default(Color),
                matrix2Up = default(Color), matrix2Upleft = default(Color), matrix2Upright = default(Color),
                matrix2Down = default(Color), matrix2Downleft = default(Color), matrix2Downright = default(Color);

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
                Math.Pow((matrix1Right.R + matrix1Rightup.R + matrix1Rightdown.R) - (matrix1Left.R + matrix1Leftup.R + matrix1Leftdown.R), 2) 
                + Math.Pow((matrix2Up.R + matrix2Upleft.R + matrix2Upright.R) - (matrix2Down.R + matrix2Downleft.R + matrix2Downright.R), 2));

            var greenDiff = (int)Math.Sqrt(
                Math.Pow((matrix1Right.G + matrix1Rightup.G + matrix1Rightdown.G) - (matrix1Left.G + matrix1Leftup.G + matrix1Leftdown.G), 2)
                + Math.Pow((matrix2Up.G + matrix2Upleft.G + matrix2Upright.G) - (matrix2Down.G + matrix2Downleft.G + matrix2Downright.G), 2));

            var blueDiff = (int)Math.Sqrt(
                Math.Pow((matrix1Right.B + matrix1Rightup.B + matrix1Rightdown.B) - (matrix1Left.B + matrix1Leftup.B + matrix1Leftdown.B), 2)
                + Math.Pow((matrix2Up.B + matrix2Upleft.B + matrix2Upright.B) - (matrix2Down.B + matrix2Downleft.B + matrix2Downright.B), 2));

            // Returing the total result

            return Math.Abs(redDiff) + Math.Abs(greenDiff) + Math.Abs(blueDiff);

        }
    }
}