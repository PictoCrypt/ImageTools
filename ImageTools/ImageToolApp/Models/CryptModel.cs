using System;
using System.Drawing;
using System.Security.Cryptography;
using FunctionLib.Cryptography;
using FunctionLib.Steganography;

namespace ImageToolApp.Models
{
    public class CryptModel<TCrypt, TStego> 
        where TCrypt : SymmetricAlgorithm
        where TStego : SteganographicAlgorithm
    {
        //public CryptModel(string src, string message, S stegano)
        //{
        //    CryptModel(src, message, null, null, stegano);
        //} 
        public CryptModel(string src, string message, string password, TCrypt encrypt, TStego stegano, int lsbIndicator)
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
            EncryptionMethod = encrypt;
            SteganographicMethod = stegano;
            LsbIndicator = lsbIndicator;
        }
        
        public string DecryptMessage
        {
            get
            {
                if (Password != null)
                {
                    return SymmetricAlgorithmBase.Encrypt(this, typeof (TCrypt), DecryptImage, Password);
                }
                else
                {
                    return DecryptImage;
                }
            }
        }

        public string DecryptImage
        {
            get { return SteganographicAlgorithmBase.Decrypt(this, typeof (TStego), Src, PasswordHash, LsbIndicator).ToString(); }
        }

        public string EncryptedMessage
        {
            get
            {
                if (Password != null)
                {
                    return SymmetricAlgorithmBase.Encrypt(this, typeof(TCrypt), Message, Password);
                }
                else
                {
                    return Message;
                }
            }
        }

        public Bitmap EncryptedImage
        {
            get
            {
                return SteganographicAlgorithmBase.Encrypt(this, typeof (TStego), Src, EncryptedMessage, PasswordHash,
                    LsbIndicator);
            }
        }

        public int LsbIndicator { get; private set; }
        private string SrcPath { get; set; }
        public Bitmap Src { get; private set; }
        public string Message { get; }
        private string Password { get; }
        public int PasswordHash { get; private set; }
        public TCrypt EncryptionMethod { get; private set; }
        public TStego SteganographicMethod { get; private set; }
    }
}