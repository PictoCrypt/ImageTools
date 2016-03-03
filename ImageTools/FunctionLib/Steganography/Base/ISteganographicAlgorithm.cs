using System.Collections.Generic;
using System.Drawing;
using FunctionLib.Helper;
using FunctionLib.Model;

namespace FunctionLib.Steganography.Base
{
    public interface ISteganographicAlgorithm
    {
        List<Pixel> ChangedPixels { get; }
        string Name { get; }
        string Description { get; }
        LockBitmap Encode(Bitmap src, MessageImpl message, int passHash, int lsbIndicator = 3);
        MessageImpl Decode(Bitmap src, int passHash, int lsbIndicator = 3);
        string ChangeColor(string srcPath, Color color);
        int MaxEmbeddingCount(Bitmap src, int lsbIndicator);
        int CheckIfEncryptionIsPossible(LockBitmap lockBitmap, int bytesLength, int significantIndicator);
    }
}