using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using FunctionLib.Helper;
using FunctionLib.Model;

namespace FunctionLib.Steganography
{
    public abstract class LsbAlgorithmBase : SteganographicAlgorithm
    {
        public void Embed(LockBitmap src, int x, int y, int significantIndicator, ref List<byte> bytes,
            ref int byteIndex, ref int bitIndex)
        {
            var pixel = src.GetPixel(x, y);
            var r = ByteHelper.ClearLeastSignificantBit(pixel.R, significantIndicator);
            var g = ByteHelper.ClearLeastSignificantBit(pixel.G, significantIndicator);
            var b = ByteHelper.ClearLeastSignificantBit(pixel.B, significantIndicator);

            r = r + CurrentByte(bytes, ref byteIndex, ref bitIndex, significantIndicator);
            g = g + CurrentByte(bytes, ref byteIndex, ref bitIndex, significantIndicator);
            b = b + CurrentByte(bytes, ref byteIndex, ref bitIndex, significantIndicator);

            src.SetPixel(x, y, Color.FromArgb(r, g, b));
            ChangedPixels.Add(new Pixel(x, y));
        }

        public void ReadEmbedded(LockBitmap src, int x, int y, int significantIndicator, ref List<byte> byteList)
        {
            var bitHolder = new List<int>();
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

        private byte CurrentByte(IReadOnlyList<byte> b, ref int byteIndex, ref int bitIndex, int significantIndicator)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < significantIndicator; i++)
            {
                if (bitIndex == 8)
                {
                    byteIndex++;
                    bitIndex = 0;
                }
                var bit = byteIndex >= b.Count ? 0 : ByteHelper.GetBit(b[byteIndex], bitIndex++);
                builder.Append(bit);
            }

            var result = Convert.ToByte(builder.ToString(), 2);
            return result;
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
    }
}