using System;
using System.Drawing;
using FunctionLib.Model.Message;

namespace FunctionLib.Steganography
{
    public class BitmapPalett : SteganographicAlgorithmImpl
    {
        // Bitmap erzeugen, Palette ändern
        //public override LockBitmap Encode(LockBitmap src, byte[] value, int password = 0,
        //    int significantIndicator = 3)
        //{
        //    throw new NotImplementedException();
        //    var lockBitmap = new LockBitmap(src.Source, PixelFormat.Format8bppIndexed);
        //    lockBitmap.LockBits();

        //    var palette = lockBitmap.Source.Palette;
        //    var paletteEntries = palette.Entries.ToList();
        //    for (var i = 0; i < value.Length; i++)
        //    {
        //        src.Source.Palette.Entries[i] = Color.FromArgb(GetValue(value, i), GetValue(value, i),
        //            GetValue(value, i));
        //        //paletteEntries.Add(Color.FromArgb(GetValue(value, i), GetValue(value, i), GetValue(value, i)));
        //    }
        //    src.Source.Palette = palette;

        //    lockBitmap.UnlockBits();
        //    return lockBitmap;
        //}

        //private int GetValue(IReadOnlyList<byte> value, int i)
        //{
        //    return i >= value.Count ? 0 : value[i];
        //}

        //protected override byte[] Decode(LockBitmap src, int password, int significantIndicator = 3)
        //{
        //    throw new NotImplementedException();
        //}

        //public override string ChangeColor(string srcPath, Color color)
        //{
        //    throw new NotImplementedException();
        //}

        //public override int MaxEmbeddingCount(int squarePixels)
        //{
        //    throw new NotImplementedException();
        //}
        public override string Name
        {
            get { return "BitmapPalett"; }
        }

        public override string Description
        {
            get { return "Changing the color palett"; }
        }

        public override Bitmap Encode(Bitmap src, ISecretMessage message, int passHash, int lsbIndicator = 3)
        {
            throw new NotImplementedException();
        }

        public override ISecretMessage Decode(Bitmap src, int passHash, int lsbIndicator = 3)
        {
            throw new NotImplementedException();
        }
    }
}