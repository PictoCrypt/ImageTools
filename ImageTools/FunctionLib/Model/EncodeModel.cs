using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using FunctionLib.Cryptography;
using FunctionLib.Helper;
using FunctionLib.Model.Message;
using FunctionLib.Steganography.Base;

namespace FunctionLib.Model
{
    public class EncodeModel
    {
        private readonly string mSrcObj;
        private readonly string mSrcMessage;
        private readonly string mPassword;
        private readonly int mLsbIndicator;
        private readonly bool mCompression;

        public EncodeModel(string imageSrc, string message, SymmetricAlgorithm crypto, string passsword, SteganographicAlgorithmImpl stegano, bool compression, int lsbIndicator)
        {
            mSrcObj = imageSrc;
            Src = FileManager.GetInstance().CopyImageToTmp(mSrcObj);
            mSrcMessage = message;
            mCompression = compression;

            //Crypt
            CryptoAlgorithm = crypto;
            mPassword = passsword;

            //Stegano
            SteganoAlgorithm = stegano;
            mLsbIndicator = lsbIndicator;
        }

        public string Src { get; set; }

        private int PasswordHash { get { return mPassword == null ? 0 : PasswordHelper.GetHash(mPassword); } }

        public SteganographicAlgorithmImpl SteganoAlgorithm { get; set; }

        public SymmetricAlgorithm CryptoAlgorithm { get; set; }

        public Bitmap Encode()
        {
            ISecretMessage message;
            if (mPassword == null || File.Exists(mSrcMessage))
            {
                if (File.Exists(mSrcMessage))
                {
                    message = new DocumentMessage(mSrcMessage, mCompression);
                }
                else
                {
                    message = new TextMessage(mSrcMessage, mCompression);
                }
            }
            else
            {
                var cryptedMessage = SymmetricAlgorithmBase.Encode(this, CryptoAlgorithm.GetType(), mSrcMessage,
                    mPassword);
                message = new TextMessage(cryptedMessage, mCompression);
            }

            Bitmap result;
            using (var bmp = new Bitmap(Src))
            {
                result = SteganoAlgorithm.Encode(bmp, message, PasswordHash, mLsbIndicator);
            }
            return result;
        }
    }
}