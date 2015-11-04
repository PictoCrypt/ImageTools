﻿using System;
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
        private readonly int[] mNullPointer = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
        private List<byte> mTextBytes;
        private int mCharIndex;
        private int mBitIndex;
        private int mSignificantIndicator;

        private int GetNextByte
        {
            get
            {
                var builder = new StringBuilder();
                for (var i = 0; i < 8 - mSignificantIndicator; i++)
                {
                    builder.Append("0");
                }
                builder.Append(GetNextBit);
                builder.Append(GetNextBit);
                builder.Append(GetNextBit);
                var result = Convert.ToInt32(builder.ToString(), 2);
                return result;
            }
        }

        private int GetNextBit
        {
            get
            {
                if (mBitIndex == 7)
                {
                    mCharIndex++;
                    mBitIndex = 0;
                }
                if (mCharIndex > mTextBytes.Count - 1)
                {
                    return 0;
                }
                return GetByte(mTextBytes[mCharIndex])[mBitIndex++];
            }
        }

        public override Bitmap Encrypt(Bitmap src, string value, int additionalParam = 3)
        {
            var result = new Bitmap(src);
            var lockBitmap = new LockBitmap(result);
            lockBitmap.LockBits();

            mCharIndex = 0;
            mBitIndex = 0;
            mSignificantIndicator = additionalParam;
            // 8 Nullen um das Ende zu erkennen
            mTextBytes = MethodHelper.StringToByteArray(value).ToList();
            mTextBytes.Add(0);
            if (mTextBytes.Count != value.Length + 1)
            {
                throw new ArgumentException("Anything failed, maybe.");
            }


            for (var y = 0; y < lockBitmap.Height; y++)
            {
                for (var x = 0; x < lockBitmap.Width; x++)
                {
                    var pixel = lockBitmap.GetPixel(x, y);
                    var r = ClearLeastSignificantBit(pixel.R, additionalParam);
                    var g = ClearLeastSignificantBit(pixel.G, additionalParam);
                    var b = ClearLeastSignificantBit(pixel.B, additionalParam);
                    
                    r = r + GetNextByte;
                    g = g + GetNextByte;
                    b = b + GetNextByte;

                    lockBitmap.SetPixel(x, y, Color.FromArgb(r, g, b));
                    ChangedPixels.Add(new Pixel(x, y));

                    if (mCharIndex > mTextBytes.Count - 1 || mCharIndex == mTextBytes.Count - 1 && mBitIndex == 7)
                    {
                        lockBitmap.UnlockBits();
                        return result;
                    }
                }
            }


            lockBitmap.UnlockBits();
            return result;
        }

        private int ClearLeastSignificantBit(int value, int lsbIndicator)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < 8 - lsbIndicator; i++)
            {
                builder.Append("1");
            }
            for (var i = 0; i < lsbIndicator; i++)
            {
                builder.Append("0");
            }

            var result = Convert.ToInt32(builder.ToString(), 2);
            return (value & result);
        }

        private int[] GetByte(int number)
        {
            var fill = "";
            var binary = Convert.ToString(number, 2);
            if (binary.Length != 8)
            {
                for (var i = 0; i < 8 -binary.Length; i++)
                {
                    fill += 0;
                }
                binary = fill + binary;
            }
            var result = new int[8];
            for (var i = 0; i < binary.Length; i++)
            {
                result[i] = int.Parse(binary.Substring(i, 1));
            }
            return result;
        }

        public override string Decrypt(Bitmap src, int additionalParam)
        {
            var builder = new StringBuilder();
            var lockBitmap = new LockBitmap(src);
            lockBitmap.LockBits();

            mCharIndex = 0;
            mBitIndex = 0;
            var listOfBits = new List<int>();
            mSignificantIndicator = additionalParam;

            for (var y = 0; y < lockBitmap.Height; y++)
            {
                for (var x = 0; x < lockBitmap.Width; x++)
                {
                    var pixel = lockBitmap.GetPixel(x, y);
                    var r = GetByte(pixel.R);
                    for (var i = r.Length - additionalParam; i < r.Length; i++)
                    {
                        listOfBits.Add(r[i]);
                    }
                    var g = GetByte(pixel.G);
                    for (var i = g.Length - additionalParam; i < g.Length; i++)
                    {
                        listOfBits.Add(g[i]);
                    }
                    var b = GetByte(pixel.B);
                    for (var i = b.Length - additionalParam; i < b.Length; i++)
                    {
                        listOfBits.Add(b[i]);
                    }

                    // Check for End (1 Byte of 0)
                    var index = IndexOf(listOfBits, mNullPointer);
                    if (index > -1)
                    {
                        var rest = index % 8;
                        var min = index + (8 - rest) + 8;
                        if (listOfBits.Count >= min)
                        {
                            listOfBits.RemoveRange(min, listOfBits.Count - min);
                            lockBitmap.UnlockBits();
                            while (listOfBits.Count > 0)
                            {
                                var range = listOfBits.GetRange(0, 8);
                                listOfBits.RemoveRange(0, 8);
                                builder.Append(GetString(range));
                            }
                            return builder.ToString();
                        }
                    }
                }
            }
            throw new SystemException("Error, anything happened (or maybe not).");
        }

        private string GetString(List<int> listOfBits)
        {
            var builder = new StringBuilder();
            foreach (var bit in listOfBits)
            {
                builder.Append(bit);
            }
            var result = (char)Convert.ToInt32(builder.ToString(), 2);
            return result.ToString();
        }

        public static int IndexOf<T>(IEnumerable<T> collection,
                                IEnumerable<T> sequence)
        {
            var ccount = collection.Count();
            var scount = sequence.Count();

            if (scount > ccount) return -1;

            if (collection.Take(scount).SequenceEqual(sequence)) return 0;

            int index = Enumerable.Range(1, ccount - scount + 1)
                                  .FirstOrDefault(i => collection.Skip(i).Take(scount).SequenceEqual(sequence));
            if (index == 0) return -1;
            return index;
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