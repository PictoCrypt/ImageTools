using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
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

            // 8 Nullen um das Ende zu erkennen
            var nullPointer = new int[8] {0, 0, 0, 0, 0, 0, 0, 0};
            textBytes = textBytes.Concat(nullPointer).ToArray();

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
                    ChangedPixels.Add(new Pixel(x, y));

                    if (charIndex + 1 == textBytes.Length)
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
            var result = new Bitmap(src);
            var lockBitmap = new LockBitmap(result);
            lockBitmap.LockBits();

            var byteList = new List<int>();

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
                        byteList.Add(r[red + 8 - additionalParam]);
                    }
                    for (var green = 0; green < additionalParam; green++)
                    {
                        byteList.Add(g[green + 8 - additionalParam]);

                    }
                    for (var blue = 0; blue < additionalParam; blue++)
                    {
                        byteList.Add(b[blue + 8 - additionalParam]);

                    }

                    if (byteList[byteList.Count - 8] == 0)
                    {
                        int zeroCounter = 0;
                        var end = byteList.GetRange(byteList.Count - 8, 8);
                        foreach (var count in end)
                        {
                            if (count == 0)
                                zeroCounter++;
                        }
                        if (zeroCounter == 8)
                        {
                            lockBitmap.UnlockBits();
                            return ConvertToString(byteList);
                        }
                    }
                }
            }
            return null;
        }

        private string ConvertToString(List<int> byteList)
        {
            var builder = new StringBuilder();
            while (byteList.Count > 0)
            {
                var byteArray = byteList.ToArray();
                var zwischen = new int[8];
                for (var i = 0; i < 8; i++)
                {
                    zwischen[i] = byteArray[i];
                }
                byteList.RemoveRange(0, 8);
                builder.Append((char)GetInt(zwischen));
                zwischen = new int[8];
            }
            return builder.ToString();
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
    }
}