using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using FunctionLib.Helper;
using FunctionLib.Model;

namespace FunctionLib
{
    public static class ImageFunctionLib
    {
        public static Bitmap Diff(Bitmap image1, Bitmap image2, int x1, int y1, int x2, int y2, int width, int height)
        {
            var diffBm = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    //Get Both Colours at the pixel point
                    var col1 = image1.GetPixel(x1 + x, y1 + y);
                    var col2 = image2.GetPixel(x2 + x, y2 + y);

                    //Get the difference RGB
                    var r = Math.Abs(col1.R - col2.R);
                    var g = Math.Abs(col1.G - col2.G);
                    var b = Math.Abs(col1.B - col2.B);

                    //Invert the difference average
                    var dif = 255 - (r + g + b)/3;

                    //Create new grayscale rgb colour
                    var newcol = Color.FromArgb(dif, dif, dif);

                    diffBm.SetPixel(x, y, newcol);
                }
            }
            return diffBm;
        }

        public static void ChangeColor(Bitmap src, Color color, IEnumerable<Pixel> changedPixels)
        {
            var lockBitmap = new LockBitmap(src);
            lockBitmap.LockBits();
            foreach (var changedPixel in changedPixels)
            {
                lockBitmap.SetPixel(changedPixel.X, changedPixel.Y, color);
            }
            lockBitmap.UnlockBits();
        }

        public static Bitmap ChangeColor(Bitmap src, Color color)
        {
            var result = new Bitmap(src);
            var lockBitmap = new LockBitmap(result);
            lockBitmap.LockBits();

            var compareClr = Color.FromArgb(255, 255, 255, 255);
            for (var y = 0; y < lockBitmap.Height; y++)
            {
                for (var x = 0; x < lockBitmap.Width; x++)
                {
                    if (lockBitmap.GetPixel(x, y) == compareClr)
                    {
                        lockBitmap.SetPixel(x, y, color);
                    }
                }
            }
            lockBitmap.UnlockBits();
            return result;
        }

        public static SortedDictionary<string, int> GettingHistogramData(Bitmap src, int index)
        {
            if (index < 1 | index > 256)
            {
                throw new ArgumentException("Index must be convertible into a byte. Number between 1 and 256.");
            }
            if (src == null)
            {
                throw new ArgumentException("src can't be null.");
            }

            var grayImage = MakeGrayscale(src);
            grayImage.Save(@"C:\Users\marius.schroeder\Desktop\Test", ImageFormat.Jpeg);
            var data = new SortedDictionary<string, int>();
            var referenceByte = Convert.ToByte(index);

            for (var y = 0; y < grayImage.Height; y++)
            {
                for (var x = 0; x < grayImage.Width; x++)
                {
                    var pixel = grayImage.GetPixel(x, y);
                    var pixelColor = pixel.R;
                    var resultingByte = (byte)(pixelColor & referenceByte);

                    if (data.ContainsKey(resultingByte.ToString()))
                    {
                        data[resultingByte.ToString()]++;
                    }
                    else
                    {
                        data.Add(resultingByte.ToString(), 1);
                    }
                }
            }
            return data;
        }

        /*
            http://stackoverflow.com/questions/2265910/convert-an-image-to-grayscale
            http://www.switchonthecode.com/tutorials/csharp-tutorial-convert-a-color-image-to-grayscale
        */
        public static Bitmap MakeGrayscale(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
               {
         new float[] {.3f, .3f, .3f, 0, 0},
         new float[] {.59f, .59f, .59f, 0, 0},
         new float[] {.11f, .11f, .11f, 0, 0},
         new float[] {0, 0, 0, 1, 0},
         new float[] {0, 0, 0, 0, 1}
               });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }
    }
}