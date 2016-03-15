using System.Collections.Generic;
using System.Drawing;
using FunctionLib.Helper;
using FunctionLib.Model;
using FunctionLib.Model.Message;

namespace FunctionLib.Steganography.Base
{
    public interface ISteganographicAlgorithm
    {
        List<Pixel> ChangedPixels { get; }
        string Name { get; }
        string Description { get; }
        Bitmap Encode(Bitmap src, ISecretMessage message, int passHash, int lsbIndicator = 3);
        ISecretMessage Decode(Bitmap src, int passHash, int lsbIndicator = 3);
        string ChangeColor(string srcPath, Color color);
        int MaxEmbeddingCount(Bitmap src, int lsbIndicator);
        int CheckIfEncryptionIsPossible(LockBitmap lockBitmap, int bytesLength, int significantIndicator);
    }
}