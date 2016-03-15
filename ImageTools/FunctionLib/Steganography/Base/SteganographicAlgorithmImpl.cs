using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using FunctionLib.Helper;
using FunctionLib.Model;
using FunctionLib.Model.Message;

namespace FunctionLib.Steganography.Base
{
    public abstract class SteganographicAlgorithmImpl : ISteganographicAlgorithm
    {
        protected SteganographicAlgorithmImpl()
        {
            ChangedPixels = new List<Pixel>();
        }

        public List<Pixel> ChangedPixels { get; }
        public abstract string Name { get; }
        public abstract string Description { get; }

        public virtual int CheckIfEncryptionIsPossible(LockBitmap lockBitmap, int bytesLength, int significantIndicator)
        {
            var pixelsAvailable = lockBitmap.Width*lockBitmap.Height;
            var bitsAvailable = pixelsAvailable*significantIndicator;
            var bitsNeeded = bytesLength*8;
            if (bitsAvailable >= bitsNeeded)
            {
                return 0;
            }
            return bitsNeeded/8;
        }

        public abstract Bitmap Encode(Bitmap src, ISecretMessage message, int passHash, int lsbIndicator = 3);

        public abstract ISecretMessage Decode(Bitmap src, int passHash, int lsbIndicator = 3);

        public virtual string ChangeColor(string srcPath, Color color)
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

        public int MaxEmbeddingCount(Bitmap src, int lsbIndicator)
        {
            // We are using the parameter leastSignificantBitIndicator each byte.
            var lsbs = src.Width*src.Height*lsbIndicator;
            // Each character uses 8 bits.
            var result = lsbs/8;
            return result;
        }

        protected LockBitmap LockBitmap(Bitmap src)
        {
            var file = new Bitmap(FileManager.GetInstance().CopyImageToTmp(src, ImageFormat.Png));
            var lockBitmap = new LockBitmap(file);
            lockBitmap.LockBits();
            return lockBitmap;
        }

        protected static ISecretMessage GetSpecificMessage(byte[] bytes)
        {
            var index = ListHelper.IndexOf(bytes, Constants.TagSeperator);
            if (index > 0)
            {
                var seq = bytes.Take(index).ToArray();
                var extension = ConvertHelper.Convert(seq);
                var result = bytes.Skip(index + 1).ToArray();
                return new DocumentMessage(result, extension);
            }
            else
            {
                return new TextMessage(bytes);
            }
        }
    }
}