using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using FunctionLib.Helper;

namespace FunctionLib.Steganography
{
    public abstract class SteganographicAlgorithm : IDisposable
    {
        public SteganographicAlgorithm()
        {
            ChangedPixels = new List<Pixel>();
        }

        protected List<Pixel> ChangedPixels { get; }

        public void Dispose()
        {
            // GNDN
        }

        protected abstract LockBitmap Encrypt(LockBitmap src, byte[] value, int significantIndicator = 3);

        public Bitmap Encrypt(Bitmap src, object value, int significantIndicator = 3)
        {
            using (var result = new Bitmap(src))
            {
                var lockBitmap = new LockBitmap(result);
                lockBitmap.LockBits();
                var bytes = MethodHelper.ToByteArray(value);
                lockBitmap = Encrypt(lockBitmap, bytes, significantIndicator);
                lockBitmap.UnlockBits();
                return result;
            }
        }

        public object Decrypt(Bitmap src, Type type, int significantIndifcator = 3)
        {
            using (var bmp = new Bitmap(src))
            {
                var lockBitmap = new LockBitmap(bmp);
                lockBitmap.LockBits();
                var bytes = Decrypt(lockBitmap, significantIndifcator).ToList();
                lockBitmap.UnlockBits();
                if (type == typeof (string))
                {
                    var builder = new StringBuilder();
                    while (bytes.Count > 0)
                    {
                        var element = bytes.First();
                        builder.Append((char)element);
                        bytes.Remove(element);
                    }
                    return builder.ToString();
                }
                if (type == typeof (Bitmap))
                {
                    //TODO: Wie erkenne ich, wo das ende einer Zeile/Spalte ist?
                }
                return null;
                
            }

        }

        protected abstract byte[] Decrypt(LockBitmap src, int significantIndicator = 3);
        public abstract string ChangeColor(string srcPath, Color color);

        public abstract int MaxEncryptionCount(int squarePixels);
    }
}