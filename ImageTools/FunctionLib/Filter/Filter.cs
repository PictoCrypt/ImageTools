using System;
using System.Drawing;
using FunctionLib.Helper;

namespace FunctionLib.Filter
{
    public abstract class Filter
    {
        protected Filter(Bitmap image, int startbits, int endbits) : this(new LockBitmap(image), startbits, endbits)
        {
        }

        protected Filter(LockBitmap image, int startbits, int endbits)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }
            Image = image;
            StartRange = startbits;
            EndRange = endbits;
            if (!image.IsLocked)
            {
                image.LockBits();
            }
        }

        protected LockBitmap Image { get; set; }
        protected int StartRange { get; set; }
        protected int EndRange { get; set; }
        public abstract int GetValue(int x, int y);
    }
}