using System;
using System.Collections.Generic;
using System.Drawing;
using FunctionLib.Enums;
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

        protected abstract LockBitmap Encrypt(LockBitmap src, byte[] value, int significantIndicator = 3);

        public Bitmap Encrypt(Bitmap src, object value, int significantIndicator = 3)
        {
            var result = new Bitmap(src);
            var lockBitmap = new LockBitmap(result);
            lockBitmap.LockBits();
            var bytes = ConvertHelper.ToByteArray(value);
            var size = CheckIfEncryptionIsPossible(lockBitmap, bytes, significantIndicator);
            if (size > 0)
            {
                throw new ArgumentOutOfRangeException(string.Format("Not enough source size. A minimum of {0} pixel is needed.", size));
            }
            lockBitmap = Encrypt(lockBitmap, bytes, significantIndicator);
            lockBitmap.UnlockBits();
            return result;
        }

        private int CheckIfEncryptionIsPossible(LockBitmap lockBitmap, byte[] bytes, int significantIndicator)
        {
            var pixelsAvailable = lockBitmap.Width*lockBitmap.Height;
            var bitsAvailable = pixelsAvailable * significantIndicator;
            var bitsNeeded = bytes.Length * 8;
            if (bitsAvailable >= bitsNeeded)
            {
                return 0;
            }
            return bitsNeeded / 8;
        }

        public object Decrypt(Bitmap src, ResultingType type, int significantIndifcator = 3)
        {
            var bmp = new Bitmap(src);
            var lockBitmap = new LockBitmap(bmp);
            lockBitmap.LockBits();
            var bytes = Decrypt(lockBitmap, significantIndifcator);
            lockBitmap.UnlockBits();
            return ConvertHelper.ToObject(bytes);
        }

        protected abstract byte[] Decrypt(LockBitmap src, int significantIndicator = 3);
        public abstract string ChangeColor(string srcPath, Color color);
        public abstract int MaxEncryptionCount(int squarePixels);
    }
}