using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using FunctionLib.CustomException;
using FunctionLib.Helper;
using FunctionLib.Model;
using FunctionLib.Model.Message;
using FunctionLib.Steganography.Base;

namespace FunctionLib.Steganography.LSB
{
    public abstract class LsbAlgorithmBase : SteganographicAlgorithmImpl
    {
        protected int BitIndex;
        protected int ByteIndex;

        public override LockBitmap Encode(Bitmap src, ISecretMessage message, int passHash, int lsbIndicator = 3)
        {
            if (src == null)
            {
                throw new ArgumentNullException(nameof(src));
            }
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            InitializeEncoding(src, message, passHash);
            if (!IsEncryptionIsPossible(src, message, lsbIndicator))
            {
                throw new ContentLengthException();
            }
            try
            {
                if (!Iteration(lsbIndicator))
                {
                    throw new SystemException();
                }
            }
            finally
            {
                CleanupEncoding();
            }
            return Bitmap;
        }

        private bool IsEncryptionIsPossible(Image src, ISecretMessage message, int lsbIndicator)
        {
            var squarePixel = src.Width*src.Height;
            var maxSize = (squarePixel*3*lsbIndicator) / 8;
            return maxSize >= Bytes.Length;
        }

        protected abstract bool Iteration(int lsbIndicator);

        protected bool CheckForEnd()
        {
            return ByteIndex > Bytes.Length - 1 || ByteIndex == Bytes.Length - 1 && BitIndex == 7;
        }

        protected void EncodeBytes(int x, int y, int lsbIndicator)
        {
            var pixel = Bitmap.GetPixel(x, y);
            var newRgbValues = ReadPixelLsb(pixel, lsbIndicator);
            Bitmap.SetPixel(x, y, Color.FromArgb(newRgbValues[0], newRgbValues[1], newRgbValues[2]));
            ChangedPixels.Add(new Pixel(x, y));
        }

        private void InitializeEncoding(Bitmap src, ISecretMessage message, int passHash)
        {
            Bitmap = LockBitmap(src);
            Bytes = message.Convert();
            PassHash = passHash;
        }

        protected int PassHash { get; private set; }

        private void CleanupEncoding()
        {
            Bitmap.UnlockBits();
        }

        protected LockBitmap Bitmap { get; private set; }

        protected List<byte> DecryptHelper(List<byte> bytes, ref ICollection<int> bitHolder)
        {
            while (bitHolder.Count >= 8)
            {
                var sequence = bitHolder.Take(8);
                var builder = new StringBuilder();
                foreach (var x in sequence)
                {
                    builder.Append(x.ToString());
                }
                bytes.Add(Convert.ToByte(builder.ToString(), 2));
                bitHolder = bitHolder.Skip(8).ToList();
            }
            return bytes;
        }

        private static byte CurrentByte(IReadOnlyList<byte> b, ref int byteIndex, ref int bitIndex, int significantIndicator)
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

        protected byte[] Bytes { get; private set; }

        private int[] ReadPixelLsb(Color pixel, int lsbIndicator)
        {
            var r = ByteHelper.ClearLeastSignificantBit(pixel.R, lsbIndicator);
            var g = ByteHelper.ClearLeastSignificantBit(pixel.G, lsbIndicator);
            var b = ByteHelper.ClearLeastSignificantBit(pixel.B, lsbIndicator);

            r = r + CurrentByte(Bytes, ref ByteIndex, ref BitIndex, lsbIndicator);
            g = g + CurrentByte(Bytes, ref ByteIndex, ref BitIndex, lsbIndicator);
            b = b + CurrentByte(Bytes, ref ByteIndex, ref BitIndex, lsbIndicator);
            return new[] {r, g, b};
        }
    }
}