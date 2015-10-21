using System.Drawing;

namespace FunctionLib.Steganography
{
    public abstract class StegaCrypt
    {
        public abstract Bitmap Encrypt(Bitmap src, string text);
        public abstract string DecryptText(Bitmap src);
    }
}