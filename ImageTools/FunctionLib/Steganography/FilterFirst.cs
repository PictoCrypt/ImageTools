using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using FunctionLib.Helper;
using FunctionLib.Filter;
using FunctionLib.Model;

namespace FunctionLib.Steganography
{
    public class FilterFirst : SteganographicAlgorithm
    {
        protected override LockBitmap Encrypt(LockBitmap src, byte[] value, int password = 0, int significantIndicator = 3)
        {
            var byteIndex = 0;
            var bitIndex = 0;
            var bytes = value.ToList();
            if (value == null)
            {
                throw new ArgumentException("'value' is null.");
            }
            src.UnlockBits();
            var filter = new Laplace(src.Source, 1, 8);
            IDictionary<Pixel, int> laplace = new Dictionary<Pixel, int>();
            for (var x = 0; x < src.Width; x++)
            {
                for (var y = 0; y < src.Height; y++)
                {
                    laplace.Add(new Pixel(x, y), filter.GetValue(x, y));
                }
            }
            src.LockBits();
            var orderedLaplace = laplace.OrderByDescending(key => key.Value);
            var random = new Random(password);

            foreach (var key in orderedLaplace)
            {
                var x = key.Key.X;
                var y = key.Key.Y;
                //var x = GetNextRandom("x", orderedLaplace.Count(), random);

                var pixel = src.GetPixel(x, y);
                var r = ByteHelper.ClearLeastSignificantBit(pixel.R, significantIndicator);
                var g = ByteHelper.ClearLeastSignificantBit(pixel.G, significantIndicator);
                var b = ByteHelper.ClearLeastSignificantBit(pixel.B, significantIndicator);

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
            throw new SystemException("Error, anything happened (or maybe not).");
        }

        protected override byte[] Decrypt(LockBitmap src, int password = 0, int significantIndicator = 3)
        {
            var random = new Random(password);
            var byteList = new List<byte>();
            var bitHolder = new List<int>();

            src.UnlockBits();
            var filter = new Laplace(src.Source, 1, 8);
            IDictionary<Pixel, int> laplace = new Dictionary<Pixel, int>();
            for (var x = 0; x < src.Width; x++)
            {
                for (var y = 0; y < src.Height; y++)
                {
                    laplace.Add(new Pixel(x, y), filter.GetValue(x, y));
                }
            }
            src.LockBits();
            var orderedLaplace = laplace.OrderByDescending(key => key.Value);


            foreach (var key in orderedLaplace)
            {
                var x = key.Key.X;
                var y = key.Key.Y;

                var pixel = src.GetPixel(x, y);
                for (var i = 0; i < significantIndicator; i++)
                {
                    var bit = ByteHelper.GetBit(pixel.R, 8 - significantIndicator + i);
                    bitHolder.Add(bit);
                }

                for (var i = 0; i < significantIndicator; i++)
                {
                    var bit = ByteHelper.GetBit(pixel.G, 8 - significantIndicator + i);
                    bitHolder.Add(bit);
                }

                for (var i = 0; i < significantIndicator; i++)
                {
                    var bit = ByteHelper.GetBit(pixel.B, 8 - significantIndicator + i);
                    bitHolder.Add(bit);
                }
                byteList = DecryptHelper(byteList, bitHolder);

                // Check for EndTag (END)
                var index = MethodHelper.IndexOfWithinLastTwo(byteList);
                if (index > -1)
                {
                    // Remove overhang bytes
                    if (byteList.Count > index + Constants.EndTag.Length)
                    {
                        byteList.RemoveRange(index + Constants.EndTag.Length,
                            byteList.Count - (index + Constants.EndTag.Length));
                    }
                    return byteList.ToArray();
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


        public override string ChangeColor(string srcPath, Color color)
        {
            var tmp = FileManager.GetInstance().GenerateTmp(ImageFormat.Png);
            var dest = FileManager.GetInstance().GenerateTmp(ImageFormat.Png);
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
            var lsbs = squarePixels * leastSignificantBitIndicator;
            // Each character uses 8 bits.
            var result = lsbs / 8;
            return result;
        }
    }
}