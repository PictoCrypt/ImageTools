using System;
using FunctionLib.Cryptography;

namespace FunctionLib.Model.Message
{
    public abstract class SecretMessage
    {
        protected readonly string mMessage;
        private string mPassword;

        protected SecretMessage(string obj, bool compression, CryptographicAlgorithmImpl crypto, string password)
        {
            if (string.IsNullOrEmpty(obj))
            {
                throw new ArgumentNullException(nameof(obj));
            }
            mMessage = obj;
            Compression = compression;
            Crypto = crypto;
            mPassword = password;
        }

        protected SecretMessage(byte[] bytes, bool compression, CryptographicAlgorithmImpl crypto, string password)
        {
            if (bytes == null || bytes.Length <= 0)
            {
                throw new ArgumentNullException(nameof(bytes));
            }
            Bytes = bytes;
            Compression = compression;
            Crypto = crypto;
            mPassword = password;
        }

        public string Password
        {
            protected get { return mPassword ?? "secret"; }
            set { mPassword = value; }
        }

        public CryptographicAlgorithmImpl Crypto { get; set; }

        public abstract string Message { get; }

        public byte[] Bytes { get; }
        public bool Compression { get; set; }

        public override string ToString()
        {
            if (Message != null)
            {
                return Message;
            }
            return base.ToString();
        }
    }
}