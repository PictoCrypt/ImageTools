using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FunctionLib.Helper;
using FunctionLib.Model;
using FunctionLib.Model.Message;

namespace FunctionLib.Steganography.LSB
{
    public class RandomLsb : LsbWithRandomness
    {
        public override string Name
        {
            get { return "HideSeek"; }
        }

        public override string Description
        {
            get { return "Randomly distributes the message across the image in the least significant bit."; }
        }

        public override LockBitmap Encode(Bitmap src, ISecretMessage message, int passHash, int lsbIndicator = 3)
        {
            var result = LockBitmap(src);
            var random = new Random(passHash);
            var byteIndex = 0;
            var bitIndex = 0;
            var bytes = message.Convert();
            while (true)
            {
                var x = GetNextRandom(Coordinate.X, src.Width, random);
                var y = GetNextRandom(Coordinate.Y, src.Height, random);

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
        }

        public override ISecretMessage Decode(Bitmap src, int passHash, MessageType type, int lsbIndicator = 3)
        {
            var random = new Random(passHash);
            var byteList = new List<byte>();
            ICollection<int> bitHolder = new List<int>();
            while (true)
            {
                var x = GetNextRandom(Coordinate.X, src.Width, random);
                var y = GetNextRandom(Coordinate.Y, src.Height, random);

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
                byteList = DecryptHelper(byteList, ref bitHolder);
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
        }
    }
}