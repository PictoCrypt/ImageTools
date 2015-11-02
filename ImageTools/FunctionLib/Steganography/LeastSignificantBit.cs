using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using FunctionLib.Helper;

namespace FunctionLib.Steganography
{
    public class LeastSignificantBit : SteganographicAlgorithm
    {
        public override Bitmap Encrypt(Bitmap src, string value, int additionalParam = 3)
        {
            var result = new Bitmap(src);
            var lockBitmap = new LockBitmap(result);
            lockBitmap.LockBits();

            for (var x = 0; x < lockBitmap.Width; x++)
            {
                for (var y = 0; y < lockBitmap.Height; y++)
                {
                    var pixel = lockBitmap.GetPixel(x, y);
                    var r = GetByte(pixel.R);
                    var g = GetByte(pixel.G);
                    var b = GetByte(pixel.B);

                    const int charIndex = 0;
                    var sign = GetByte((int)value[charIndex]);
                    for (var red = 0; red < additionalParam; red++)
                    {
                        r[red + 8 - additionalParam] = sign[red];
                    }
                    for (var green = 0; green < additionalParam; green++)
                    {
                        g[green + 8 - additionalParam] = sign[3 + green];
                    }
                    for (var blue = 0; blue < additionalParam; blue++)
                    {
                        b[blue + 8 - additionalParam] = sign[6 + blue];
                    }
                }
            }


            lockBitmap.UnlockBits();
            return result;
        }

        private int[] GetByte(int number)
        {
            var rest = number;
            var result = new int[8] { 128, 64, 32, 16, 8, 4, 2, 1 };
            var fillBit = false;

            for (var i = 0; i < result.Length; i++)
            {
                if (fillBit)
                {
                    result[i] = 0;
                    continue;
                }

                rest = rest % result[i];
                if (rest != number)
                {
                    result[i] = 1;
                    if (rest == 0)
                    {
                        fillBit = true;
                        continue;
                    }
                }
                else
                {
                    result[i] = 0;
                }
            }
            return result;
        }

        public override string Decrypt(Bitmap src, int additionalParam = 3)
        {
            var lockBitmap = new LockBitmap(new Bitmap(src));
            lockBitmap.LockBits();

            var colorUnitIndex = 0;
            var charValue = 0;

            // holds the value that will be extracted from the image
            var result = new StringBuilder();

            // pass through the rows
            for (var i = 0; i < lockBitmap.Height; i++)
            {
                // pass through each row
                for (var j = 0; j < lockBitmap.Width; j++)
                {
                    var pixel = lockBitmap.GetPixel(j, i);

                    // for each pixel, pass through its elements (RGB) = 3
                    for (var n = 0; n < 3; n++)
                    {
                        switch (colorUnitIndex % 3)
                        {
                            case 0:
                            {
                                // get the LSB from the pixel element (will be pixel.R % 2)
                                // then add one bit to the right of the current character
                                // this can be done by (charValue = charValue * 2)
                                // replace the added bit (which value is by default 0) with
                                // the LSB of the pixel element, simply by addition
                                charValue = charValue *2 + pixel.R % 2;
                            }
                                break;
                            case 1:
                            {
                                charValue = charValue *2 + pixel.G % 2;
                            }
                                break;
                            case 2:
                            {
                                charValue = charValue *2 + pixel.B % 2;
                            }
                                break;
                        }

                        colorUnitIndex++;

                        // if 8 bits has been added,
                        // then add the current character to the result value
                        if (colorUnitIndex % 8 == 0)
                        {
                            // reverse? of course, since each time the process occurs
                            // on the right (for simplicity)
                            charValue = ReverseBits(charValue);

                            // can only be 0 if it is the stop character (the 8 zeros)
                            if (charValue == 0)
                            {
                                return result.ToString();
                            }

                            // convert the character value from int to char
                            var c = (char) charValue;

                            // add the current character to the result value
                            result.Append(c);;
                        }
                    }
                }
            }

            lockBitmap.UnlockBits();
            return result.ToString();
        }

        public override string ChangeColor(string srcPath, Color color)
        {
            var tmp = Path.GetTempFileName();
            var dest = Path.GetTempFileName();
            File.Copy(srcPath, tmp, true);
            using (var bitmap = new Bitmap(tmp))
            {
                ImageFunctionLib.ChangeColor(bitmap, color, ChangedPixels);
                bitmap.Save(dest, ImageFormat.Bmp);
            }
            File.Copy(dest, tmp, true);
            return tmp;
        }

        private static int ReverseBits(int n)
        {
            var result = 0;

            for (var i = 0; i < 8; i++)
            {
                result = result *2 + n % 2;

                n /= 2;
            }

            return result;
        }

        private enum State
        {
            Hiding,
            FillingWithZeros
        };
    }
}