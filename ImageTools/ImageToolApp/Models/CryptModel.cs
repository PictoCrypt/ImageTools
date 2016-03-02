using System;
using System.Drawing;
using FunctionLib.Cryptography;
using FunctionLib.Steganography;

namespace ImageToolApp.Models
{
    public class CryptModel //<TCrypt, TStego>
        //    where TCrypt : SymmetricAlgorithm
        //    where TStego : SteganographicAlgorithm
    {
        //public CryptModel(string src, string message, S stegano)
        //{
        //    CryptModel(src, message, null, null, stegano);
        //} 
        public CryptModel(string src, string message, string password, Type encryptType, Type steganoType,
            int lsbIndicator)
        {
            if (string.IsNullOrEmpty(src))
            {
                throw new ArgumentException("Src can not be null or empty.");
            }
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentException("Message can not be null or empty.");
            }


            SrcPath = src;
            //TODO: tmp?
            Src = new Bitmap(src);
            Message = message;
            Password = password;
            //TODO: verbesserte Methode
            PasswordHash = Password == null ? 0 : password.GetHashCode();
            EncryptionMethodType = encryptType;
            SteganographicMethodType = steganoType;
            LsbIndicator = lsbIndicator;
        }

        private Type SteganographicMethodType { get; }
        private Type EncryptionMethodType { get; }

        public string DecryptMessage
        {
            get
            {
                if (Password != null)
                {
                    return SymmetricAlgorithmBase.Encrypt(this, EncryptionMethodType, DecryptImage, Password);
                }
                return DecryptImage;
            }
        }

        public string DecryptImage
        {
            get
            {
                return
                    SteganographicAlgorithmBase.Decrypt(this, SteganographicMethodType, Src, PasswordHash, LsbIndicator)
                        .ToString();
            }
        }

        public string EncryptedMessage
        {
            get
            {
                if (Password != null)
                {
                    return SymmetricAlgorithmBase.Encrypt(this, EncryptionMethodType, Message, Password);
                }
                return Message;
            }
        }

        public Bitmap EncryptedImage
        {
            get
            {
                return SteganographicAlgorithmBase.Encrypt(this, SteganographicMethodType, Src, EncryptedMessage,
                    PasswordHash,
                    LsbIndicator);
            }
        }

        public int LsbIndicator { get; }
        private string SrcPath { get; set; }
        public Bitmap Src { get; }
        public string Message { get; }
        private string Password { get; }
        public int PasswordHash { get; }
        //public TStego SteganographicMethod { get; private set; }
        //public TCrypt EncryptionMethod { get; private set; }
    }
}