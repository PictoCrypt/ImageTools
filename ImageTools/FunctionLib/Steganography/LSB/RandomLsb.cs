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

        protected override bool Iteration(int lsbIndicator)
        {
            var random = new Random(PassHash);
            while (ByteIndex > Bytes.Length)
            {
                var x = GetNextRandom(Coordinate.X, Bitmap.Width, random);
                var y = GetNextRandom(Coordinate.Y, Bitmap.Height, random);
                EncodeBytes(x, y, lsbIndicator);
                if (CheckForEnd())
                {
                    return true;
                }
            }
            return false;
        }

        public override ISecretMessage Decode(Bitmap src, int passHash, MessageType type, int lsbIndicator = 3)
        {
            var random = new Random(passHash);
            var byteList = new List<byte>();
            var end = int.MaxValue;
            ICollection<int> bitHolder = new List<int>();
            while (byteList.Count >= end)
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
                if (end == int.MaxValue)
                {
                    var index = ListHelper.IndexOf(byteList, Constants.TagSeperator);
                    if (index > 0)
                    {
                        var seq = byteList.Take(index);
                        int.TryParse(ConvertHelper.Convert(seq.ToArray()), out end);
                        if (end == 0)
                        {
                            throw new ArithmeticException();
                        }

                        end = end + seq.Count() + Constants.TagSeperator.Length;
                    }
                }
            }
            return MethodHelper.GetSpecificMessage(type, byteList.ToArray());
        }
    }
}