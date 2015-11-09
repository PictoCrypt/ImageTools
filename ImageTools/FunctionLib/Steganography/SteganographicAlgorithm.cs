﻿using System;
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
            //using (var result = new Bitmap(src))
            {
                var result = new Bitmap(src);
                var lockBitmap = new LockBitmap(result);
                lockBitmap.LockBits();
                var bytes = MethodHelper.ToByteArray(value).Concat(NullByte).ToArray();
                lockBitmap = Encrypt(lockBitmap, bytes, significantIndicator);
                lockBitmap.UnlockBits();
                return result;
            }
        }

        protected readonly byte[] NullByte = MethodHelper.StringToByteArray("<EOF>").ToArray();

        public object Decrypt(Bitmap src, ResultingType type, int significantIndifcator = 3)
        {
            //using (var bmp = new Bitmap(src))
            {
                var bmp = new Bitmap(src);
                var lockBitmap = new LockBitmap(bmp);
                lockBitmap.LockBits();
                var bytes = Decrypt(lockBitmap, significantIndifcator);
                lockBitmap.UnlockBits();
                if (type == ResultingType.Text)
                {
                    return Encoding.GetEncoding("ISO-8859-1").GetString(bytes);
                    //return Encoding.UTF8.GetString(bytes);
                }
                if (type == ResultingType.Image)
                {
                    //TODO: Wie erkenne ich, wo das ende einer Zeile/Spalte ist?
                }
                if (type == ResultingType.Document)
                {
                    
                }
                return null;
            }
        }

        protected abstract byte[] Decrypt(LockBitmap src, int significantIndicator = 3);
        public abstract string ChangeColor(string srcPath, Color color);

        public abstract int MaxEncryptionCount(int squarePixels);
    }
}