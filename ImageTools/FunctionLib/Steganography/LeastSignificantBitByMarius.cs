using System.Drawing;
using System.Linq;
using FunctionLib.Helper;

namespace FunctionLib.Steganography
{
    public class LeastSignificantBitByMarius : SteganographicAlgorithm
    {
        public override Bitmap Encrypt(Bitmap src, string value, int additionalParam = 3)
        {
            var result = new Bitmap(src);
            var lockBitmap = new LockBitmap(result);
            lockBitmap.LockBits();

            var charIndex = 0;
            int[] textBytes = null;
            foreach (var obj in value)
            {
                var charByte = GetByte(obj);
                if (textBytes == null)
                    textBytes = charByte;
                else
                    textBytes = textBytes.Concat(charByte).ToArray();
            }

            for (var x = 0; x < lockBitmap.Width; x++)
            {
                for (var y = 0; y < lockBitmap.Height; y++)
                {
                    var pixel = lockBitmap.GetPixel(x, y);
                    var r = GetByte(pixel.R);
                    var g = GetByte(pixel.G);
                    var b = GetByte(pixel.B);

                    for (var red = 0; red < additionalParam; red++)
                    {
                        if (charIndex + 1 < textBytes.Length)
                        {
                            r[red + 8 - additionalParam] = textBytes[charIndex++];
                        }
                    }
                    for (var green = 0; green < additionalParam; green++)
                    {
                        if (charIndex + 1 < textBytes.Length)
                        {
                            g[green + 8 - additionalParam] = textBytes[charIndex++];
                        }
                    }
                    for (var blue = 0; blue < additionalParam; blue++)
                    {
                        if (charIndex + 1 < textBytes.Length)
                        {
                            b[blue + 8 - additionalParam] = textBytes[charIndex++];
                        }
                    }
                    lockBitmap.SetPixel(x, y, Color.FromArgb(GetInt(r), GetInt(g), GetInt(b)));

                    if (charIndex + 1 < textBytes.Length)
                    {
                        lockBitmap.UnlockBits();
                        return result;
                    }
                }
            }


            lockBitmap.UnlockBits();
            return result;
        }

        private int GetInt(int[] b)
        {
            var b1 = new int[8] { 128, 64, 32, 16, 8, 4, 2, 1 };
            var result = 0;
            for (var i = 0; i < b.Length; i++)
            {
                result += b1[i] * b[i];
            }
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

        public override string Decrypt(Bitmap src, int additionalParam)
        {
            throw new System.NotImplementedException();
        }

        public override string ChangeColor(string srcPath, Color color)
        {
            throw new System.NotImplementedException();
        }
    }
}