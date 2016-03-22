using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using FunctionLib.Helper;
using FunctionLib.Model.Message;

namespace FunctionLib.Steganography
{
    public abstract class SteganographicAlgorithmImpl
    {
        public abstract string Name { get; }
        public abstract string Description { get; }

        protected FileManager FileManager { get { return FileManager.GetInstance(); } }

        public abstract string Encode(string src, ISecretMessage message, int passHash, int lsbIndicator = 3);

        public abstract ISecretMessage Decode(string src, int passHash, int lsbIndicator = 3);

        public abstract IList<ImageFormat> PossibleImageFormats { get; }

        public abstract int MaxEmbeddingCount(Bitmap src, int lsbIndicator);

        protected LockBitmap LockBitmap(string src)
        {
            var lockBitmap = new LockBitmap(new Bitmap(src));
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
            return new TextMessage(bytes);
        }

        public override string ToString()
        {
            return Name;
        }

        public override int GetHashCode()
        {
            var result = Name.GetHashCode() + Description.GetHashCode();
            return result;
        }

        public override bool Equals(object obj)
        {
            var other = obj as SteganographicAlgorithmImpl;
            if (other == null)
            {
                return false;
            }

            if (Name.Equals(other.Name))
            {
                return true;
            }
            return false;
        }
    }
}