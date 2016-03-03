using System;
using System.Collections.Generic;
using System.Drawing;
using FunctionLib.Helper;
using FunctionLib.Model;

namespace FunctionLib.Steganography.LSB
{
    public class LeastSignificantBit : LsbAlgorithmBase
    {
        public override string Name
        {
            get { return "BlindHide"; }
        }

        public override string Description
        {
            get
            {
                return "Blindy hides the secret message in the least significant bits," +
                       " starting from the top left corner and working across the image pixel by pixel.";
            }
        }

        public override LockBitmap Encode(Bitmap src, MessageImpl message, int passHas, int lsbIndicator = 3)
        {
            var result = LockBitmap(src);
            var byteIndex = 0;
            var bitIndex = 0;
            var bytes = message.GetMessageAsBytes();

            for (var y = 0; y < result.Height; y++)
            {
                for (var x = 0; x < result.Width; x++)
                {
                    var pixel = result.GetPixel(x, y);
                    var r = ByteHelper.ClearLeastSignificantBit(pixel.R, lsbIndicator);
                    var g = ByteHelper.ClearLeastSignificantBit(pixel.G, lsbIndicator);
                    var b = ByteHelper.ClearLeastSignificantBit(pixel.B, lsbIndicator);

                    r = r + CurrentByte(bytes, ref byteIndex, ref bitIndex, lsbIndicator);
                    g = g + CurrentByte(bytes, ref byteIndex, ref bitIndex, lsbIndicator);
                    b = b + CurrentByte(bytes, ref byteIndex, ref bitIndex, lsbIndicator);

                    result.SetPixel(x, y, Color.FromArgb(r, g, b));
                    ChangedPixels.Add(new Pixel(x, y));
                    if (byteIndex > bytes.Length - 1 || byteIndex == bytes.Length - 1 && bitIndex == 7)
                    {
                        return result;
                    }
                }
            }
            return result;
        }

        public override MessageImpl Decode(Bitmap src, int passHash, int lsbIndicator = 3)
        {
            var byteList = new List<byte>();
            for (var y = 0; y < src.Height; y++)
            {
                for (var x = 0; x < src.Width; x++)
                {
                    var bitHolder = new List<int>();
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
                        return new MessageImpl(byteList.ToArray());
                    }
                }
            }
            throw new SystemException("Error, anything happened (or maybe not).");
        }
    }
}