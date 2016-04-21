using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using FunctionLib.Helper;
using FunctionLib.Model;
using FunctionLib.Model.Message;

namespace FunctionLib.Steganography.LSB
{
    public abstract class LsbAlgorithmBase : SteganographicAlgorithmImpl
    {
        protected int BitIndex;
        protected int ByteIndex;

        public LsbAlgorithmBase()
        {
            ChangedPixels = new List<Pixel>();
        }

        protected int EndCount { get; set; }

        protected ICollection<int> BitHolder { get; set; }

        protected byte[] Bytes { get; set; }

        public override IList<ImageFormat> PossibleImageFormats
        {
            get { return new List<ImageFormat> {ImageFormat.Bmp, ImageFormat.Png, ImageFormat.MemoryBmp}; }
        }

        public List<Pixel> ChangedPixels { get; }

        protected int LsbIndicator { get; set; }

        protected override string EncodingAlgorithm(string src, ISecretMessage message, int passHash)
        {
            var tmp = FileManager.CopyImageToTmp(src);

            try
            {
                if (!EncodingIteration())
                {
                    throw new SystemException();
                }
            }
            finally
            {
                Cleanup();
                Bitmap.Source.Save(tmp);
            }
            return tmp;
        }

        protected override ISecretMessage DecodingAlgorithm(string src, int lsbIndicator)
        {
            try
            {
                if (!DecodingIteration())
                {
                    throw new SystemException();
                }
            }
            finally
            {
                Cleanup();
            }
            
            return GetSpecificMessage(Bytes.ToArray());
        }

        private void RemoveSizeTag()
        {
            var index = ListHelper.IndexOf(Bytes, Constants.TagSeperator);
            Bytes = Bytes.Skip(index + 1).ToArray();
        }

        protected abstract bool DecodingIteration();

        protected override void InitializeDecoding(string src, int passHash, int lsbIndicator)
        {
            base.InitializeDecoding(src, passHash, lsbIndicator);
            Bytes = new byte[0];
            BitHolder = new List<int>();
            EndCount = int.MaxValue;
        }

        public override int MaxEmbeddingCount(Bitmap src, int lsbIndicator)
        {
            // We are using the parameter leastSignificantBitIndicator each byte.
            var lsbs = src.Width*src.Height*lsbIndicator;
            // Each character uses 8 bits.
            var result = lsbs/8;
            return result;
        }

        protected abstract bool EncodingIteration();

        protected bool EncodeCheckForEnd()
        {
            return ByteIndex > Bytes.Length - 1 || ByteIndex == Bytes.Length - 1 && BitIndex == 7;
        }

        protected override bool IsEncryptionPossible()
        {
            var squarePixel = Bitmap.Width*Bitmap.Height;
            var maxSize = squarePixel*3*LsbIndicator/8;
            return maxSize >= Bytes.Length;
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

                    RemoveSizeTag();
                }
            }

            if (Bytes.Length >= EndCount)
            {
                Bytes = Bytes.Take(EndCount).ToArray();
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

        protected override void InitializeEncoding(string src, ISecretMessage message, int passHash, int lsbIndicator)
        {
            base.InitializeEncoding(src, message, passHash, lsbIndicator);
            LsbIndicator = lsbIndicator;
            BitIndex = 0;
            ByteIndex = 0;
            Bytes = message.Convert();
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

        public virtual string ChangeColor(string srcPath, Color color)
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