using System.IO;
using FunctionLib.Cryptography;
using FunctionLib.Helper;
using FunctionLib.Model.Message;
using FunctionLib.Steganography;

namespace FunctionLib.Model
{
    public class EncodeModel
    {
        private readonly bool mCompression;
        private readonly int mLsbIndicator;
        private readonly string mPassword;
        private readonly string mSrcMessage;
        private readonly string mSrcObj;

        public EncodeModel(string imageSrc, string message, CryptographicAlgorithmImpl crypto, string passsword,
            SteganographicAlgorithmImpl stegano, bool compression, int lsbIndicator)
        {
            mSrcObj = imageSrc;
            mSrcMessage = message;
            mCompression = compression;

            //Crypt
            CryptoAlgorithm = crypto;
            mPassword = passsword;

            //Stegano
            SteganoAlgorithm = stegano;
            mLsbIndicator = lsbIndicator;
        }

        private int PasswordHash
        {
            get { return string.IsNullOrEmpty(mPassword) ? 0 : PasswordHelper.GetHash(mPassword); }
        }

        public SteganographicAlgorithmImpl SteganoAlgorithm { get; set; }

        public CryptographicAlgorithmImpl CryptoAlgorithm { get; set; }

        public string Encode()
        {
            ISecretMessage message;
            if (File.Exists(mSrcMessage))
            {
                message = new DocumentMessage(mSrcMessage, mCompression, CryptoAlgorithm, mPassword);
            }
            else
            {
                message = new TextMessage(mSrcMessage, mCompression, CryptoAlgorithm, mPassword);
            }

            var result = SteganoAlgorithm.Encode(mSrcObj, message, PasswordHash, mLsbIndicator);
            return result;
        }
    }
}