using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FunctionLib.Helper;
using FunctionLib.Model.Message;

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

        protected override bool Iteration(int lsbIndicator)
        {
            for (var y = 0; y < Bitmap.Height; y++)
            {
                for (var x = 0; x < Bitmap.Width; x++)
                {
                    EncodeBytes(x, y, lsbIndicator);
                    if (CheckForEnd())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override ISecretMessage Decode(Bitmap src, int passHash, MessageType type, int lsbIndicator = 3)
        {
            var img = LockBitmap(src);
            var byteList = new List<byte>();
            ICollection<int> bitHolder = new List<int>();
            var end = int.MaxValue;
            for (var y = 0; y < img.Height; y++)
            {
                for (var x = 0; x < img.Width; x++)
                {
                    var pixel = img.GetPixel(x, y);
                    int bit;
                    for (var i = 0; i < lsbIndicator; i++)
                    {
                        bit = ByteHelper.GetBit(pixel.R, 8 - lsbIndicator + i);
                        bitHolder.Add(bit);
                    }

                    for (var i = 0; i < lsbIndicator; i++)
                    {
                        bit = ByteHelper.GetBit(pixel.G, 8 - lsbIndicator + i);
                        bitHolder.Add(bit);
                    }

                    for (var i = 0; i < lsbIndicator; i++)
                    {
                        bit = ByteHelper.GetBit(pixel.B, 8 - lsbIndicator + i);
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

                    if (byteList.Count >= end)
                    {
                        return MethodHelper.GetSpecificMessage(type, byteList.ToArray());
                    }
                }
            }
            throw new SystemException("Error, anything happened (or maybe not).");
        }
    }
}