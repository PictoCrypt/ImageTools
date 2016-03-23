using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using FunctionLib.CustomException;
using FunctionLib.Helper;
using FunctionLib.Model.Message;

namespace FunctionLib.Steganography
{
    public abstract class SteganographicAlgorithmImpl
    {
        public abstract string Name { get; }
        public abstract string Description { get; }

        protected FileManager FileManager
        {
            get { return FileManager.GetInstance(); }
        }

        protected LockBitmap Bitmap { get; set; }

        protected int PassHash { get; set; }

        public abstract IList<ImageFormat> PossibleImageFormats { get; }

        public string Encode(string src, ISecretMessage message, int passHash, int lsbIndicator = 3)
        {
            if (string.IsNullOrEmpty(src))
            {
                throw new ArgumentNullException(nameof(src));
            }
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            if (lsbIndicator < 0 || lsbIndicator > 8)
            {
                throw new ArgumentException(nameof(lsbIndicator));
            }

            InitializeEncoding(src, message, passHash, lsbIndicator);

            if (!IsEncryptionPossible())
            {
                CleanupEncoding();
                throw new ContentLengthException();
            }
            if (!DoesCoverFitType())
            {
                CleanupEncoding();
                throw new BadImageFormatException();
            }

            var result = EncodingAlgorithm(src, message, passHash, lsbIndicator);

            CleanupEncoding();

            return result;
        }

        private bool DoesCoverFitType()
        {
            var format = FileManager.GetImageFormat(Bitmap.Source.RawFormat);
            return PossibleImageFormats.Contains(format);
        }

        private void CleanupEncoding()
        {
            Bitmap.Source.Dispose();
        }

        protected abstract bool IsEncryptionPossible();

        protected virtual void InitializeEncoding(string src, ISecretMessage message, int passHash, int lsbIndicator)
        {
            Bitmap = LockBitmap(src);
            PassHash = passHash;
        }

        protected abstract string EncodingAlgorithm(string src, ISecretMessage message, int passHash, int lsbIndicator);

        public ISecretMessage Decode(string src, int passHash, int lsbIndicator = 3)
        {
            if (string.IsNullOrEmpty(src))
            {
                throw new ArgumentNullException(nameof(src));
            }

            InitializeDecoding(src, passHash, lsbIndicator);

            var result = DecodingAlgorithm(src, passHash, lsbIndicator);
            return result;
        }

        protected virtual void InitializeDecoding(string src, int passHash, int lsbIndicator)
        {
            Bitmap = LockBitmap(src);
            PassHash = passHash;
        }

        protected abstract ISecretMessage DecodingAlgorithm(string src, int passHash, int lsbIndicator);

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