using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using FunctionLib.Model.Message;

namespace FunctionLib.Steganography
{
    public class BitmapPalett : SteganographicAlgorithmImpl
    {
        // Bitmap erzeugen, Palette ändern
        //public override LockBitmap Encode(LockBitmap src, byte[] value, int password = 0,
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
            get { return new List<ImageFormat> {ImageFormat.Bmp, ImageFormat.Gif, ImageFormat.Png}; }
        }

        protected override bool IsEncryptionPossible()
        {
            throw new NotImplementedException();
        }

        protected override string EncodingAlgorithm(string src, ISecretMessage message, int passHash,
            int lsbIndicator = 3)
        {
            throw new NotImplementedException();
        }

        protected override ISecretMessage DecodingAlgorithm(string src, int passHash, int lsbIndicator)
        {
            throw new NotImplementedException();
        }

        public override int MaxEmbeddingCount(Bitmap src, int lsbIndicator)
        {
            return int.MaxValue;
        }
    }
}