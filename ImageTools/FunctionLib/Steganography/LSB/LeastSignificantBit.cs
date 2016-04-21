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

        protected override bool EncodingIteration()
        {
            for (var y = 0; y < Bitmap.Height; y++)
            {
                for (var x = 0; x < Bitmap.Width; x++)
                {
                    EncodeBytes(x, y, LsbIndicator);
                    if (EncodeCheckForEnd())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected override bool DecodingIteration()
        {
            for (var y = 0; y < Bitmap.Height; y++)
            {
                for (var x = 0; x < Bitmap.Width; x++)
                {
                    DecodeBytes(x, y, LsbIndicator);
                    //TODO: Fix this? Why is this so fucking cumbersome? Cant REF BitHolder
                    var bitHolder = BitHolder;
                    Bytes = BitToByte(Bytes, ref bitHolder);
                    BitHolder = bitHolder;
                    if (DecodeCheckForEnd())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}