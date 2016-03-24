using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using FunctionLib.Helper;
using FunctionLib.Model.Message;

namespace FunctionLib.Steganography
{
    public class BitmapPalett : SteganographicAlgorithmImpl
    {
        // Bitmap erzeugen, Palette ändern
        public override string Name
        {
            get { return "BitmapPalett"; }
        }

        public override string Description
        {
            get { return "Changing the color palett"; }
        }

        public override IList<ImageFormat> PossibleImageFormats
        {
            get { return Constants.ImageFormats; }
        }

        protected override bool IsEncryptionPossible()
        {
            return Bitmap.Source.Palette.Entries.Length > 0;
        }

        protected override string EncodingAlgorithm(string src, ISecretMessage message, int passHash,
            int lsbIndicator)
        {
            var palette = Bitmap.Source.Palette;
            throw new NotImplementedException();
        }

        protected override ISecretMessage DecodingAlgorithm(string src, int lsbIndicator)
        {
            throw new NotImplementedException();
        }

        public override int MaxEmbeddingCount(Bitmap src, int lsbIndicator)
        {
            return int.MaxValue;
        }
    }
}