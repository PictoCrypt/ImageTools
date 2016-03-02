using System;
using System.Collections.Generic;
using System.Drawing;
using FunctionLib.Helper;
using FunctionLib.Model;

namespace FunctionLib.Steganography
{
    public abstract class SteganographicAlgorithm : IDisposable
    {
        public SteganographicAlgorithm()
        {
            ChangedPixels = new List<Pixel>();
        }

        protected internal List<Pixel> ChangedPixels { get; }

        public void Dispose()
        {
            // GNDN
        }

        protected abstract LockBitmap Encrypt(LockBitmap src, byte[] value, int password = 0, int significantIndicator = 3);

        public Bitmap Encrypt(Bitmap src, string value, int password = 0, int significantIndicator = 3)
        {
            var result = new Bitmap(src);
            var lockBitmap = new LockBitmap(result);
            lockBitmap.LockBits();
            var bytes = ConvertHelper.Convert(value);
            var size = CheckIfEncryptionIsPossible(lockBitmap, bytes, significantIndicator);
            if (size > 0)
            {
                throw new ArgumentOutOfRangeException(
                    string.Format("Not enough source size. A minimum of {0} pixel is needed.", size));
            }
            lockBitmap = Encrypt(lockBitmap, bytes, password, significantIndicator);
            lockBitmap.UnlockBits();
            return result;
        }

        private int CheckIfEncryptionIsPossible(LockBitmap lockBitmap, byte[] bytes, int significantIndicator)
        {
            var pixelsAvailable = lockBitmap.Width*lockBitmap.Height;
            var bitsAvailable = pixelsAvailable*significantIndicator;
            var bitsNeeded = bytes.Length*8;
            if (bitsAvailable >= bitsNeeded)
            {
                return 0;
            }
            return bitsNeeded/8;
        }

        public object Decrypt(Bitmap src, int password = 0, int significantIndifcator = 3)
        {
            var bmp = new Bitmap(src);
            var lockBitmap = new LockBitmap(bmp);
            lockBitmap.LockBits();
            var bytes = Decrypt(lockBitmap, password, significantIndifcator);
            lockBitmap.UnlockBits();
            return ConvertHelper.ConvertBack(bytes);
        }

        protected abstract byte[] Decrypt(LockBitmap src, int password = 0, int significantIndicator = 3);
        public abstract string ChangeColor(string srcPath, Color color);
        public abstract int MaxEncryptionCount(int squarePixels);
    }
}