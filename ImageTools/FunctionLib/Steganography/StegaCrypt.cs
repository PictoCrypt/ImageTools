using System.Collections.Generic;
using System.Drawing;
using FunctionLib.Helper;

namespace FunctionLib.Steganography
{
    public abstract class StegaCrypt
    {
        protected List<Pixel> ChangedPixels { get; }

        public StegaCrypt()
        {
            ChangedPixels = new List<Pixel>();
        }

        public abstract Bitmap Encrypt(Bitmap src, string text);
        public abstract string DecryptText(Bitmap src);
        public abstract string ChangeColor(string srcPath, Color color);

    }
}