using System;

namespace FunctionLib.Model.Message
{
    public class SecretMessage
    {
        protected SecretMessage(string obj, bool compression)
        {
            if (string.IsNullOrEmpty(obj))
            {
                throw new ArgumentNullException(nameof(obj));
            }
            Message = obj;
            Compression = compression;
        }

        protected SecretMessage(byte[] bytes, bool compression)
        {
            if (bytes == null || bytes.Length <= 0)
            {
                throw new ArgumentNullException(nameof(bytes));
            }
            Bytes = bytes;
            Compression = compression;
        }

        public string Message { get; }
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