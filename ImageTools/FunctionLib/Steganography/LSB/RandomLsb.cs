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

        protected override bool EncodingIteration()
        {
            while (ByteIndex < Bytes.Length)
            {
                var pixel = GetNextRandom(Bitmap.Width, Bitmap.Height, Random);
                EncodeBytes(pixel.X, pixel.Y, LsbIndicator);
                if (EncodeCheckForEnd())
                {
                    return true;
                }
            }
            return false;
        }

        protected override bool DecodingIteration()
        {
            while (Bytes.Length <= EndCount)
            {
                var pixel = GetNextRandom(Bitmap.Width, Bitmap.Height, Random);
                DecodeBytes(pixel.X, pixel.Y, LsbIndicator);
                //TODO: Fix this? Why is this so fucking cumbersome? Cant REF BitHolder
                var bitHolder = BitHolder;
                Bytes = BitToByte(Bytes, ref bitHolder);
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