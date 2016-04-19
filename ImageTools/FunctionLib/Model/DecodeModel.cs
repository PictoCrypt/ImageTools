using FunctionLib.Cryptography;
using FunctionLib.Helper;
using FunctionLib.Steganography;

namespace FunctionLib.Model
{
    public class DecodeModel
    {
        private readonly bool mCompression;
        private readonly int mLsbIndicator;
        private readonly string mPassword;
        private readonly string mSrcObj;

        public DecodeModel(string imageSrc, CryptographicAlgorithmImpl crypto, string passsword,
            SteganographicAlgorithmImpl stegano, bool compression, int lsbIndicator)
        {
            mSrcObj = imageSrc;
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
            get { return mPassword == null ? 0 : PasswordHelper.GetHash(mPassword); }
        }

        public SteganographicAlgorithmImpl SteganoAlgorithm { get; }

        public CryptographicAlgorithmImpl CryptoAlgorithm { get; }

        public string Decode()
        {
            var result = SteganoAlgorithm.Decode(mSrcObj, PasswordHash, mLsbIndicator);
            result.Crypto = CryptoAlgorithm;
            result.Password = mPassword;
            result.Compression = mCompression;
            var message = result.ConvertBack();
            return message;
        }
    }
}