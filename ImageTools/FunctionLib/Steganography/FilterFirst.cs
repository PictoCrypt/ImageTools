using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using FunctionLib.Filter;
using FunctionLib.Helper;
using FunctionLib.Model;

namespace FunctionLib.Steganography
{
    public class FilterFirst : LsbAlgorithmBase
    {
        protected override LockBitmap Encrypt(LockBitmap src, byte[] value, int password = 0,
            int significantIndicator = 3)
        {
            if (value == null)
            {
                throw new ArgumentException("'value' is null.");
            }
            src.UnlockBits();
            var filter = new Laplace(src.Source, 1, 8);
            IDictionary<Pixel, int> laplace = new Dictionary<Pixel, int>();
            for (var x = 0; x < src.Width; x++)
            {
                for (var y = 0; y < src.Height; y++)
                {
                    laplace.Add(new Pixel(x, y), filter.GetValue(x, y));
                }
            }
            src.LockBits();
            var orderedLaplace = laplace.OrderByDescending(key => key.Value);
            //var random = new Random(password);

            var byteIndex = 0;
            var bitIndex = 0;
            var bytes = value.ToList();
            foreach (var key in orderedLaplace)
            {
                var x = key.Key.X;
                var y = key.Key.Y;
                //var x = GetNextRandom("x", orderedLaplace.Count(), random);

                Embed(src, x, y, significantIndicator, ref bytes, ref byteIndex, ref bitIndex);
                if (byteIndex > bytes.Count - 1 || byteIndex == bytes.Count - 1 && bitIndex == 7)
                {
                    return src;
                }
            }
            throw new SystemException("Error, anything happened (or maybe not).");
        }

        protected override byte[] Decrypt(LockBitmap src, int password = 0, int significantIndicator = 3)
        {
            //var random = new Random(password);

            src.UnlockBits();
            var filter = new Laplace(src.Source, 1, 8);
            IDictionary<Pixel, int> laplace = new Dictionary<Pixel, int>();
            for (var x = 0; x < src.Width; x++)
            {
                for (var y = 0; y < src.Height; y++)
                {
                    laplace.Add(new Pixel(x, y), filter.GetValue(x, y));
                }
            }
            src.LockBits();
            var orderedLaplace = laplace.OrderByDescending(key => key.Value);


            var byteList = new List<byte>();
            var bitHolder = new List<int>();
            foreach (var key in orderedLaplace)
            {
                var x = key.Key.X;
                var y = key.Key.Y;

                ReadEmbedded(src, x, y, significantIndicator, ref byteList);

                // Check for EndTag (END)
                var index = MethodHelper.IndexOfWithinLastTwo(byteList);
                if (index > -1)
                {
                    // Remove overhang bytes
                    if (byteList.Count > index + Constants.EndTag.Length)
                    {
                        byteList.RemoveRange(index + Constants.EndTag.Length,
                            byteList.Count - (index + Constants.EndTag.Length));
                    }
                    return byteList.ToArray();
                }
            }
            throw new SystemException("Error, anything happened (or maybe not).");
        }

        public override string ChangeColor(string srcPath, Color color)
        {
            var tmp = FileManager.GetInstance().GenerateTmp(ImageFormat.Png);
            var dest = FileManager.GetInstance().GenerateTmp(ImageFormat.Png);
            File.Copy(srcPath, tmp, true);
            using (var bitmap = new Bitmap(tmp))
            {
                ImageFunctionLib.ChangeColor(bitmap, color, ChangedPixels);
                bitmap.Save(dest, ImageFormat.Bmp);
            }
            File.Copy(dest, tmp, true);
            return tmp;
        }
    }
}