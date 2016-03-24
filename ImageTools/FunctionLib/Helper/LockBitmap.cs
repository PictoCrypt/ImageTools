using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace FunctionLib.Helper
{
    public class LockBitmap
    {
        private readonly PixelFormat mFormat;
        private BitmapData mBitmapData;
        private IntPtr mIptr = IntPtr.Zero;

        public LockBitmap(Bitmap source, PixelFormat format = PixelFormat.Undefined)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (format == PixelFormat.Undefined)
            {
                mFormat = source.PixelFormat;
            }
            else
            {
                mFormat = format;
            }

            Source = source;
        }

        public bool IsLocked { get; set; }
        private byte[] Pixels { get; set; }
        private int Depth { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Bitmap Source { get; }

        /// <summary>
        ///     Lock bitmap data
        /// </summary>
        public void LockBits()
        {
            try
            {
                // Get width and height of bitmap
                Width = Source.Width;
                Height = Source.Height;

                // get total locked pixels count
                var pixelCount = Width*Height;

                // Create rectangle to lock
                var rect = new Rectangle(0, 0, Width, Height);

                // get source bitmap pixel format size
                Depth = Image.GetPixelFormatSize(mFormat);

                // Check if bpp (Bits Per Pixel) is 8, 24, or 32
                if (Depth != 8 && Depth != 24 && Depth != 32)
                {
                    throw new ArgumentException("Only 8, 24 and 32 bpp images are supported.");
                }

                // Lock bitmap and return bitmap data
                mBitmapData = Source.LockBits(rect, ImageLockMode.ReadWrite,
                    mFormat);

                // create byte array to copy pixel values
                var step = Depth/8;
                Pixels = new byte[pixelCount*step];
                mIptr = mBitmapData.Scan0;

                // Copy data from pointer to array
                Marshal.Copy(mIptr, Pixels, 0, Pixels.Length);
                IsLocked = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Unlock bitmap data
        /// </summary>
        public void UnlockBits()
        {
            try
            {
                // Copy data from byte array to pointer
                Marshal.Copy(Pixels, 0, mIptr, Pixels.Length);

                // Unlock bitmap data
                Source.UnlockBits(mBitmapData);
                IsLocked = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Get the color of the specified pixel
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Color GetPixel(int x, int y)
        {
            var clr = Color.Empty;

            // Get color components count
            var cCount = Depth/8;

            // Get start index of the specified pixel
            var i = (y*Width + x)*cCount;

            if (i > Pixels.Length - cCount)
                throw new IndexOutOfRangeException();

            if (Depth == 32) // For 32 bpp get Red, Green, Blue and Alpha
            {
                var b = Pixels[i];
                var g = Pixels[i + 1];
                var r = Pixels[i + 2];
                var a = Pixels[i + 3]; // a
                clr = Color.FromArgb(a, r, g, b);
            }
            if (Depth == 24) // For 24 bpp get Red, Green and Blue
            {
                var b = Pixels[i];
                var g = Pixels[i + 1];
                var r = Pixels[i + 2];
                clr = Color.FromArgb(r, g, b);
            }
            if (Depth == 8)
                // For 8 bpp get color value (Red, Green and Blue values are the same)
            {
                var c = Pixels[i];
                clr = Color.FromArgb(c, c, c);
            }
            return clr;
        }

        /// <summary>
        ///     Set the color of the specified pixel
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        public void SetPixel(int x, int y, Color color)
        {
            // Get color components count
            var cCount = Depth/8;

            // Get start index of the specified pixel
            var i = (y*Width + x)*cCount;

            if (Depth == 32) // For 32 bpp set Red, Green, Blue and Alpha
            {
                Pixels[i] = color.B;
                Pixels[i + 1] = color.G;
                Pixels[i + 2] = color.R;
                Pixels[i + 3] = color.A;
            }
            if (Depth == 24) // For 24 bpp set Red, Green and Blue
            {
                Pixels[i] = color.B;
                Pixels[i + 1] = color.G;
                Pixels[i + 2] = color.R;
            }
            if (Depth == 8)
                // For 8 bpp set color value (Red, Green and Blue values are the same)
            {
                Pixels[i] = color.B;
            }
        }
    }
}