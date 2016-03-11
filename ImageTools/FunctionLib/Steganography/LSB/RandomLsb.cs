using System;
using System.Linq;

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

        protected override bool EncodingIteration(int lsbIndicator)
        {
            var random = new Random(PassHash);
            while (ByteIndex < Bytes.Length)
            {
                var x = GetNextRandom(Coordinate.X, Bitmap.Width, random);
                var y = GetNextRandom(Coordinate.Y, Bitmap.Height, random);
                EncodeBytes(x, y, lsbIndicator);
                if (EncodeCheckForEnd())
                {
                    return true;
                }
            }
            return false;
        }

        protected override bool DecodingIteration(int lsbIndicator)
        {
            var random = new Random(PassHash);
            while (Bytes.Length <= EndCount)
            {
                var x = GetNextRandom(Coordinate.X, Bitmap.Width, random);
                var y = GetNextRandom(Coordinate.Y, Bitmap.Height, random);
                DecodeBytes(x, y, lsbIndicator);
                //TODO: Fix this? Why is this so fucking cumbersome? Cant REF BitHolder
                var bitHolder = BitHolder;
                Bytes = BitToByte(Bytes.ToList(), ref bitHolder);
                BitHolder = bitHolder;
                if (DecodeCheckForEnd())
                {
                    return true;
                }
            }
            return false;
        }
    }
}