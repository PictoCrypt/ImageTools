using System;
using System.Drawing;
using FunctionLib.Helper;
using FunctionLib.Filter;

namespace FunctionLib.Steganography
{
    public class FilterFirst : SteganographicAlgorithm
    {
        protected override LockBitmap Encrypt(LockBitmap src, byte[] value, int password = 0, int significantIndicator = 3)
        {
            //random short picker
            // Messagesize
            var random = new Random(password);
            //Size in the first 32 Bit
            //Embed - solange nicht fertig, nächsten Shot holen (Zufallszahl)
            return null;
        }

        protected override byte[] Decrypt(LockBitmap src, int password = 0, int significantIndicator = 3)
        {
            throw new System.NotImplementedException();
        }

        public override string ChangeColor(string srcPath, Color color)
        {
            throw new System.NotImplementedException();
        }

        public override int MaxEncryptionCount(int squarePixels)
        {
            return MaxEncryptionCount(squarePixels, 3);
        }

        public int MaxEncryptionCount(int squarePixels, int leastSignificantBitIndicator)
        {
            // We are using the parameter leastSignificantBitIndicator each byte.
            var lsbs = squarePixels * leastSignificantBitIndicator;
            // Each character uses 8 bits.
            var result = lsbs / 8;
            return result;
        }
    }
}