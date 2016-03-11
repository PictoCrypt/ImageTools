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

        protected int EndCount { get; set; }

        protected ICollection<int> BitHolder { get; set; }

        protected int PassHash { get; private set; }

        protected LockBitmap Bitmap { get; private set; }

        protected byte[] Bytes { get; set; }

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
            if (!IsEncryptionIsPossible(src, lsbIndicator))
            {
                throw new ContentLengthException();
            }
            try
            {
                if (!EncodingIteration(lsbIndicator))
                {
                    throw new SystemException();
                }
            }
            finally
            {
                Cleanup();
            }
            return Bitmap;
        }

        public override ISecretMessage Decode(Bitmap src, int passHash, MessageType type, int lsbIndicator = 3)
        {
            if (src == null)
            {
                throw new ArgumentNullException(nameof(src));
            }
            InitializeDecoding(src, passHash);

            try
            {
                if (!DecodingIteration(lsbIndicator))
                {
                    throw new SystemException();
                }
            }
            finally
            {
                Cleanup();
            }
            return GetSpecificMessage(type, Bytes.ToArray());
        }

        protected abstract bool DecodingIteration(int lsbIndicator);

        private void InitializeDecoding(Bitmap src, int passHash)
        {
            Bitmap = LockBitmap(src);
            PassHash = passHash;
            Bytes = new byte[0];
            BitHolder = new List<int>();
            EndCount = int.MaxValue;
        }

        private bool IsEncryptionIsPossible(Image src, int lsbIndicator)
        {
            var squarePixel = src.Width*src.Height;
            var maxSize = squarePixel*3*lsbIndicator/8;
            return maxSize >= Bytes.Length;
        }

        protected abstract bool EncodingIteration(int lsbIndicator);

        protected bool EncodeCheckForEnd()
        {
            return ByteIndex > Bytes.Length - 1 || ByteIndex == Bytes.Length - 1 && BitIndex == 7;
        }

        protected bool DecodeCheckForEnd()
        {
            // Check for EndTag (END)
            if (EndCount == int.MaxValue)
            {
                var index = ListHelper.IndexOf(Bytes, Constants.TagSeperator);
                if (index > 0)
                {
                    var seq = Bytes.Take(index);
                    //TODO: Fix this? Why is this so fucking cumbersome? Cant REF BitHolder
                    int endCount;
                    int.TryParse(ConvertHelper.Convert(seq.ToArray()), out endCount);
                    EndCount = endCount;
                    if (EndCount == 0)
                    {
                        throw new ArithmeticException();
                    }

                    EndCount = EndCount + seq.Count() + Constants.TagSeperator.Length;
                }
            }

            if (Bytes.Length >= EndCount)
            {
                return true;
            }
            return false;
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

        private void Cleanup()
        {
            Bitmap.UnlockBits();
        }

        protected byte[] BitToByte(byte[] bytes, ref ICollection<int> bitHolder)
        {
            return bytes == null ? BitToByte(new List<byte>(), ref bitHolder) : BitToByte(bytes.ToList(), ref bitHolder);
        }

        protected byte[] BitToByte(List<byte> bytes, ref ICollection<int> bitHolder)
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
            return bytes.ToArray();
        }

        private static byte CurrentByte(IReadOnlyList<byte> b, ref int byteIndex, ref int bitIndex,
            int significantIndicator)
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

        protected void DecodeBytes(int x, int y, int lsbIndicator)
        {
            var pixel = Bitmap.GetPixel(x, y);
            int bit;
            for (var i = 0; i < lsbIndicator; i++)
            {
                bit = ByteHelper.GetBit(pixel.R, 8 - lsbIndicator + i);
                BitHolder.Add(bit);
            }

            for (var i = 0; i < lsbIndicator; i++)
            {
                bit = ByteHelper.GetBit(pixel.G, 8 - lsbIndicator + i);
                BitHolder.Add(bit);
            }

            for (var i = 0; i < lsbIndicator; i++)
            {
                bit = ByteHelper.GetBit(pixel.B, 8 - lsbIndicator + i);
                BitHolder.Add(bit);
            }
        }
    }
}