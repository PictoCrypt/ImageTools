using System.Collections.Generic;
using System.Drawing.Imaging;
using FunctionLib.Helper;

namespace FunctionLib.Steganography.LSB
{
    public class ExtendedLsb : LsbAlgorithmBase
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
            for (var y = 0; y < Bitmap.Height; y++)
            {
                for (var x = 0; x < Bitmap.Width; x++)
                {
                    if (!ImageHelper.TransparentOrWhite(Bitmap.GetPixel(x, y)))
                    {
                        EncodeBytes(x, y, lsbIndicator);
                        if (EncodeCheckForEnd())
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        protected override bool DecodingIteration(int lsbIndicator)
        {
            for (var y = 0; y < Bitmap.Height; y++)
            {
                for (var x = 0; x < Bitmap.Width; x++)
                {
                    if (!ImageHelper.TransparentOrWhite(Bitmap.GetPixel(x, y)))
                    {
                        DecodeBytes(x, y, lsbIndicator);
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
            }
            return false;
        }

        public override IList<ImageFormat> PossibleImageFormats { get { return Constants.ImageFormats; } }
    }
}