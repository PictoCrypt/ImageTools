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
                    "Extends the random LSB-Algorithm by functions like checking if the pixel is white or transparent.";
            }
        }

        protected override bool EncodingIteration()
        {
            while (!EncodeCheckForEnd())
            {
                var pixel = GetNextRandom(Bitmap.Width, Bitmap.Height, Random);
                if (!ImageHelper.TransparentOrWhite(Bitmap.GetPixel(pixel.X, pixel.Y), LsbIndicator))
                {
                    //TODO check if we can give pixel here
                    EncodeBytes(pixel.X, pixel.Y, LsbIndicator);
                }                
            }
            return true;
        }

        protected override bool DecodingIteration()
        {
            while (!DecodeCheckForEnd())
            {
                var pixel = GetNextRandom(Bitmap.Width, Bitmap.Height, Random);
                if (!ImageHelper.TransparentOrWhite(Bitmap.GetPixel(pixel.X, pixel.Y), LsbIndicator))
                {
                    DecodeBytes(pixel.X, pixel.Y, LsbIndicator);
                    //TODO: Fix this? Why is this so fucking cumbersome? Cant REF BitHolder
                    var bitHolder = BitHolder;
                    Bytes = BitToByte(Bytes, ref bitHolder);
                    BitHolder = bitHolder;
                }
            }
            return Bytes.Length == EndCount;
        }
    }
}