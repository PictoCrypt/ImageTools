using System.Drawing;
using System.Security.Cryptography;
using FunctionLib.Cryptography;
using FunctionLib.Helper;
using FunctionLib.Model.Message;
using FunctionLib.Steganography.Base;

namespace FunctionLib.Model
{
    public class DecodeModel
    {
        private readonly string mSrcObj;
        private readonly string mPassword;
        private readonly int mLsbIndicator;
        private readonly bool mCompression;
        private readonly MessageType mType;

        public DecodeModel(string imageSrc, MessageType type, SymmetricAlgorithm crypto, string passsword, SteganographicAlgorithmImpl stegano, bool compression, int lsbIndicator)
        {
            mSrcObj = imageSrc;
            Src = FileManager.GetInstance().CopyImageToTmp(mSrcObj);
            mCompression = compression;
            mType = type;

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

        public string Decode()
        {
            ISecretMessage result;
            using (var bmp = new Bitmap(Src))
            {
                result = SteganoAlgorithm.Decode(bmp, PasswordHash, mType, mLsbIndicator);
            }
            //TODO: Kompression einbauen
            var message = SymmetricAlgorithmBase.Decode(this, CryptoAlgorithm.GetType(), result.ConvertBack(),
                    mPassword);
            return message;
        }
    }
}