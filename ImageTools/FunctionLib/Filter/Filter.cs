using System;
using System.Drawing;
using FunctionLib.Helper;

namespace FunctionLib.Filter
{
    /*
    *   This code is based on the DIIT steganography project, which is available at the following address: http://diit.sourceforge.net/
    */
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
            ByteMask = GetByteMask();
            if (!image.IsLocked)
            {
                image.LockBits();
            }
        }

        private int ByteMask { get; }

        protected int GetRed(Color color)
        {
            return color.R & ByteMask;
        }

        protected int GetGreen(Color color)
        {
            return color.G & ByteMask;
        }

        protected int GetBlue(Color color)
        {
            return color.B & ByteMask;
        }

        private int GetByteMask()
        {
            int abyte = 0, abyte2 = 0;
            for (var i = 0; i < 8; i++)
            {
                byte bit;
                if (i <= EndRange && i >= StartRange)
                    bit = 1;
                else
                    bit = 0;
                abyte = (byte)(abyte << 1 | bit);
            }
            for (var i = 0; i < 8; i++)
            {
                abyte2 = abyte2 << 1 | ((abyte >> i) & 0x1);
            }
            return abyte2;
        }

        protected LockBitmap Image { get; }
        private int StartRange { get; }
        private int EndRange { get; }
        public abstract int GetValue(int x, int y);
    }
}