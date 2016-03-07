using System;
using System.IO.Compression;

namespace FunctionLib.Model.Message
{
    public class SecretMessage
    {
        public SecretMessage(string obj, CompressionLevel compression)
        {
            if (string.IsNullOrEmpty(obj))
            {
                throw new ArgumentNullException(nameof(obj));
            }
            Message = obj;
            CompressionLevel = compression;
        }

        public SecretMessage(byte[] bytes, CompressionLevel compression)
        {
            if (bytes == null || bytes.Length <= 0)
            {
                throw new ArgumentNullException(nameof(bytes));
            }
            Bytes = bytes;
            CompressionLevel = compression;
        }

        public string Message { get; set; }
        public byte[] Bytes { get; set; }
        public CompressionLevel CompressionLevel { get; }
    }
}