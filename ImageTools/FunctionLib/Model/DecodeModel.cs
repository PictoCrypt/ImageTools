﻿using System.Drawing;
using FunctionLib.Cryptography;
using FunctionLib.Helper;
using FunctionLib.Model.Message;
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
            Src = FileManager.GetInstance().CopyImageToTmp(mSrcObj);
            mCompression = compression;

            //Crypt
            CryptoAlgorithm = crypto;
            mPassword = passsword;

            //Stegano
            SteganoAlgorithm = stegano;
            mLsbIndicator = lsbIndicator;
        }

        public string Src { get; set; }

        private int PasswordHash
        {
            get { return mPassword == null ? 0 : PasswordHelper.GetHash(mPassword); }
        }

        public SteganographicAlgorithmImpl SteganoAlgorithm { get; }

        public CryptographicAlgorithmImpl CryptoAlgorithm { get; }

        public string Decode()
        {
            ISecretMessage result;
            using (var bmp = new Bitmap(Src))
            {
                result = SteganoAlgorithm.Decode(bmp, PasswordHash, mLsbIndicator);
            }
            result.Compression = mCompression;
            var message = CryptoAlgorithm.Decode(result.ConvertBack(), mPassword);
            return message;
        }
    }
}