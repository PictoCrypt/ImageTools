using System.Linq;
using FunctionLib.Helper;

namespace FunctionLib.Steganography.LSB
{
    public class ExtendedLsb : LsbWithRandomness
    {
        public override string Name
        {
            get { return "Extended LSB"; }
        }

        public override string Description
        {
            get
            {
                return
                    "Extends the normal LSB-Algorithm by functions like checking if the pixel is white or transparent.";
            }
        }

        protected override bool EncodingIteration(int lsbIndicator)
        {
            while (ByteIndex < Bytes.Length)
            {
                var pixel = GetNextRandom(Bitmap.Width, Bitmap.Height, Random);
                if (!ImageHelper.TransparentOrWhite(Bitmap.GetPixel(pixel.X, pixel.Y)))
                {
                    EncodeBytes(pixel.X, pixel.Y, lsbIndicator);
                    if (EncodeCheckForEnd())
                    {
                        return true;
                    }
                    
                }
            }
            return false;
        }

        protected override bool DecodingIteration(int lsbIndicator)
        {
            while (Bytes.Length <= EndCount)
            {
                var pixel = GetNextRandom(Bitmap.Width, Bitmap.Height, Random);
                if (!ImageHelper.TransparentOrWhite(Bitmap.GetPixel(pixel.X, pixel.Y)))
                {
                    DecodeBytes(pixel.X, pixel.Y, lsbIndicator);
                    //TODO: Fix this? Why is this so fucking cumbersome? Cant REF BitHolder
                    var bitHolder = BitHolder;
                    Bytes = BitToByte(Bytes.ToList(), ref bitHolder);
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