using System;
using System.Drawing;
using System.Net.WebSockets;
using FunctionLib.Cryptography;
using FunctionLib.Model;
using FunctionLib.Model.Message;
using FunctionLib.Steganography.Base;

namespace ImageToolApp.Models
{
    public class CryptModel
    {
        public CryptModel(string src, string message, Type steganoType, int lsbIndicator = 3)
            : this(src, message, null, null, steganoType, lsbIndicator)
        {
        }

        public CryptModel(string src, string message, string password, Type encryptType, Type steganoType,
            int lsbIndicator = 3)
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
                //TODO
                return
                    SteganographicAlgorithmBase.Decrypt(this, SteganographicMethodType, Src, PasswordHash, MessageType.Text, LsbIndicator)
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
                //TODO
                return SteganographicAlgorithmBase.Encrypt(this, SteganographicMethodType, Src, new TextMessage(EncryptedMessage), 
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
    }
}