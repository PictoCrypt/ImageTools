using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FunctionLib.Filter;
using FunctionLib.Helper;
using FunctionLib.Model;
using FunctionLib.Model.Message;

namespace FunctionLib.Steganography.LSB
{
    public class FilterFirst : LsbAlgorithmBase
    {
        public override string Name
        {
            get { return "FilterFirst"; }
        }

        public override string Description
        {
            get
            {
                return "The algorithm filters the image using an edge-detection filter " +
                       "like \"Laplace-Filter\". Hiding the secret information into the " +
                       "pixels with the highest filter-values.";
            }
        }

        public override LockBitmap Encode(Bitmap src, ISecretMessage message, int passHash, int lsbIndicator = 3)
        {
            var filter = new Laplace(src, 1, 8);
            IDictionary<Pixel, int> laplace = new Dictionary<Pixel, int>();
            for (var x = 0; x < src.Width; x++)
            {
                for (var y = 0; y < src.Height; y++)
                {
                    laplace.Add(new Pixel(x, y), filter.GetValue(x, y));
                }
            }

            var result = LockBitmap(src);
            var orderedLaplace = laplace.OrderByDescending(key => key.Value);
            //var random = new Random(password);

            var byteIndex = 0;
            var bitIndex = 0;
            var bytes = message.Convert();
            foreach (var key in orderedLaplace)
            {
                var x = key.Key.X;
                var y = key.Key.Y;
                //var x = GetNextRandom("x", orderedLaplace.Count(), random);

                var pixel = src.GetPixel(x, y);
                var r = ByteHelper.ClearLeastSignificantBit(pixel.R, lsbIndicator);
                var g = ByteHelper.ClearLeastSignificantBit(pixel.G, lsbIndicator);
                var b = ByteHelper.ClearLeastSignificantBit(pixel.B, lsbIndicator);

                r = r + CurrentByte(bytes, ref byteIndex, ref bitIndex, lsbIndicator);
                g = g + CurrentByte(bytes, ref byteIndex, ref bitIndex, lsbIndicator);
                b = b + CurrentByte(bytes, ref byteIndex, ref bitIndex, lsbIndicator);

                src.SetPixel(x, y, Color.FromArgb(r, g, b));
                ChangedPixels.Add(new Pixel(x, y));
                if (byteIndex > bytes.Length - 1 || byteIndex == bytes.Length - 1 && bitIndex == 7)
                {
                    return result;
                }
            }
            throw new SystemException("Error, anything happened (or maybe not).");
        }

        public override ISecretMessage Decode(Bitmap src, int passHash, MessageType type, int lsbIndicator = 3)
        {
            var filter = new Laplace(src, 1, 8);
            IDictionary<Pixel, int> laplace = new Dictionary<Pixel, int>();
            for (var x = 0; x < src.Width; x++)
            {
                for (var y = 0; y < src.Height; y++)
                {
                    laplace.Add(new Pixel(x, y), filter.GetValue(x, y));
                }
            }

            var result = LockBitmap(src);
            var orderedLaplace = laplace.OrderByDescending(key => key.Value);

            var byteList = new List<byte>();
            var bitHolder = new List<int>();
            foreach (var key in orderedLaplace)
            {
                var x = key.Key.X;
                var y = key.Key.Y;

                var pixel = src.GetPixel(x, y);
                for (var i = 0; i < lsbIndicator; i++)
                {
                    var bit = ByteHelper.GetBit(pixel.R, 8 - lsbIndicator + i);
                    bitHolder.Add(bit);
                }

                for (var i = 0; i < lsbIndicator; i++)
                {
                    var bit = ByteHelper.GetBit(pixel.G, 8 - lsbIndicator + i);
                    bitHolder.Add(bit);
                }

                for (var i = 0; i < lsbIndicator; i++)
                {
                    var bit = ByteHelper.GetBit(pixel.B, 8 - lsbIndicator + i);
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
                    return MethodHelper.GetSpecificMessage(type, byteList.ToArray());
                }
            }
            throw new SystemException("Error, anything happened (or maybe not).");
        }
    }
}