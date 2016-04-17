using System;
using FunctionLib.Cryptography;

namespace FunctionLib.Model.Message
{
    public abstract class SecretMessage
    {
        protected readonly string mMessage;
        private readonly string mPassword;

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

        protected string Password
        {
            get { return mPassword ?? "secret"; }
        }

        protected CryptographicAlgorithmImpl Crypto { get; set; }

        protected SecretMessage(byte[] bytes, bool compression, CryptographicAlgorithmImpl crypto, string password)
        {
            if (bytes == null || bytes.Length <= 0)
            {
                throw new ArgumentNullException(nameof(bytes));
            }
            Bytes = bytes;
            Compression = compression;
            mPassword = password;
        }

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