using System;
using System.Collections.Generic;
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
        protected override LockBitmap Encrypt(LockBitmap src, byte[] value, int significantIndicator = 3)
        {
            var byteIndex = 0;
            var bitIndex = 0;
            var bytes = value.ToList();
            if (value == null)
            {
                throw new ArgumentException("'value' is null.");
            }

            if (bytes.Count != value.Length + 1)
            {
                throw new ArgumentException("Anything failed, maybe.");
            }

            for (var y = 0; y < src.Height; y++)
            {
                for (var x = 0; x < src.Width; x++)
                {
                    var pixel = src.GetPixel(x, y);
                    var r = ClearLeastSignificantBit(pixel.R, significantIndicator);
                    var g = ClearLeastSignificantBit(pixel.G, significantIndicator);
                    var b = ClearLeastSignificantBit(pixel.B, significantIndicator);

                    r = r + CurrentByte(bytes, ref byteIndex, ref bitIndex, significantIndicator);
                    g = g + CurrentByte(bytes, ref byteIndex, ref bitIndex, significantIndicator);
                    b = b + CurrentByte(bytes, ref byteIndex, ref bitIndex, significantIndicator);

                    src.SetPixel(x, y, Color.FromArgb(r, g, b));
                    ChangedPixels.Add(new Pixel(x, y));

                    if (byteIndex > bytes.Count - 1 || byteIndex == bytes.Count - 1 && bitIndex == 7)
                    {
                        return src;
                    }
                }
            }
            return src;
        }

        /// <summary>
        ///     Gets the bit of this byte on a specific position.
        /// </summary>
        /// <param name="b">Byte</param>
        /// <param name="index">Index. Index of 0 is the most significant bit.</param>
        /// <returns></returns>
        private int GetBit(byte b, int index)
        {
            var builder = new StringBuilder("00000000");
            builder.Remove(index, 1);
            builder.Insert(index, 1);

            var result = Convert.ToInt32(builder.ToString(), 2);
            return (b & result) > 0 ? 1 : 0;

            //var x = Math.Pow(2, 7 - index);
            //var bit = b & Convert.ToByte(x);
            //return bit > 0 ? 1 : 0;

            //var bit = (b & (1 >> index - 1));
            //return bit;
        }

        private byte CurrentByte(List<byte> b, ref int byteIndex, ref int bitIndex, int significantIndicator)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < significantIndicator; i++)
            {
                if (bitIndex == 8)
                {
                    byteIndex++;
                    bitIndex = 0;
                }
                if (byteIndex >= b.Count)
                {
                    return 0;
                }
                var bit = GetBit(b[byteIndex], bitIndex++);
                builder.Append(bit);
            }

            var result = Convert.ToByte(builder.ToString(), 2);
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
            return value & result;
        }

        protected override byte[] Decrypt(LockBitmap src, int significantIndicator = 3)
        {
            var byteList = new List<byte>();
            var bitHolder = new List<int>();

            for (var y = 0; y < src.Height; y++)
            {
                for (var x = 0; x < src.Width; x++)
                {
                    var pixel = src.GetPixel(x, y);

                    for (var i = 0; i < significantIndicator; i++)
                    {
                        var bit = GetBit(pixel.R, 8 - significantIndicator + i);
                        bitHolder.Add(bit);
                    }

                    for (var i = 0; i < significantIndicator; i++)
                    {
                        var bit = GetBit(pixel.G, 8 - significantIndicator + i);
                        bitHolder.Add(bit);
                    }

                    for (var i = 0; i < significantIndicator; i++)
                    {
                        var bit = GetBit(pixel.B, 8 - significantIndicator + i);
                        bitHolder.Add(bit);
                    }

                    byteList = DecryptHelper(byteList, bitHolder);


                    // Check for NullByte (END)
                    //TODO: Wie erkennen wir ob es ein Bild oder ein Text oder Dokument ist?
                    var index = IndexOf(byteList, NullByte);
                    if (index > -1)
                    {
                        if (byteList.Count - 1 > index + NullByte.Length)
                        {
                            // Remove all other elements after the nullbyte
                            for (var i = index + NullByte.Length; i < byteList.Count - 1; i++)
                            {
                                byteList.RemoveAt(i);
                            }
                        }
                        return byteList.ToArray();
                    }
                }
            }
            throw new SystemException("Error, anything happened (or maybe not).");
        }

        /// <summary>
        ///     Summarizing 8 bits to 1 byte and adding to the bytes list.
        /// </summary>
        /// <param name="bytes">List for holding the ended bytes.</param>
        /// <param name="bitHolder">List for holding the bits.</param>
        /// <returns></returns>
        private List<byte> DecryptHelper(List<byte> bytes, ICollection<int> bitHolder)
        {
            var builder = new StringBuilder();
            while (bitHolder.Count >= 8 - builder.Length)
            {
                var value = bitHolder.First();
                bitHolder.Remove(value);
                builder.Append(value);
                if (builder.Length == 8)
                {
                    var result = Convert.ToByte(builder.ToString(), 2);
                    builder = new StringBuilder();
                    bytes.Add(result);
                }
            }
            return bytes;
        }

        /// <summary>
        /// Returns the Index of the first element of this sequence.
        /// If this sequence isnt in the collection, -1 is returned.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection to search in.</param>
        /// <param name="sequence">The sequence which is searched for.</param>
        /// <returns></returns>
        public static int IndexOf<T>(IEnumerable<T> collection, IEnumerable<T> sequence)
        {
            var ccount = collection.Count();
            var scount = sequence.Count();

            if (scount > ccount) return -1;

            if (collection.Take(scount).SequenceEqual(sequence)) return 0;

            var index = Enumerable.Range(1, ccount - scount + 1)
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

        public override int MaxEncryptionCount(int squarePixels)
        {
            return MaxEncryptionCount(squarePixels, 3);
        }

        public int MaxEncryptionCount(int squarePixels, int leastSignificantBitIndicator)
        {
            // We are using the parameter leastSignificantBitIndicator each byte.
            var lsbs = squarePixels*leastSignificantBitIndicator;
            // Each character uses 8 bits.
            var result = lsbs/8;
            return result;
        }
    }
}