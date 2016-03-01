using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using FunctionLib.Helper;

namespace FunctionLib.Steganography
{
    public class BitmapPalett : SteganographicAlgorithm
    {
        // Bitmap erzeugen, Palette ändern
        protected override LockBitmap Encrypt(LockBitmap src, byte[] value, string password = null, int significantIndicator = 3)
        {
            throw new NotImplementedException();
            var lockBitmap = new LockBitmap(src.Source, PixelFormat.Format8bppIndexed);
            lockBitmap.LockBits();

            var palette = lockBitmap.Source.Palette;
            var paletteEntries = palette.Entries.ToList();
            for (var i = 0; i < value.Length; i++)
            {
                src.Source.Palette.Entries[i] = Color.FromArgb(GetValue(value, i), GetValue(value, i),
                    GetValue(value, i));
                //paletteEntries.Add(Color.FromArgb(GetValue(value, i), GetValue(value, i), GetValue(value, i)));
            }
            src.Source.Palette = palette;

            lockBitmap.UnlockBits();
            return lockBitmap;
        }

        private int GetValue(IReadOnlyList<byte> value, int i)
        {
            return i >= value.Count ? 0 : value[i];
        }

        protected override byte[] Decrypt(LockBitmap src, string password, int significantIndicator = 3)
        {
            throw new NotImplementedException();
        }

        public override string ChangeColor(string srcPath, Color color)
        {
            throw new NotImplementedException();
        }

        public override int MaxEncryptionCount(int squarePixels)
        {
            throw new NotImplementedException();
        }
    }
}