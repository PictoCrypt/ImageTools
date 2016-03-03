using System;
using FunctionLib.Helper;

namespace FunctionLib.Model
{
    public class MessageImpl
    {
        public MessageImpl(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException("message can not be null or empty.");
            }
            Message = message;
        }

        public MessageImpl(byte[] encryptedMessage)
        {
            if (encryptedMessage == null || encryptedMessage.Length < 1)
            {
                throw new ArgumentNullException("encryptedMessage can not be null or empty.");
            }
            Bytes = encryptedMessage;
        }

        private string Message { get; }
        private byte[] Bytes { get; }

        public byte[] GetMessageAsBytes()
        {
            var result = ConvertHelper.Convert(Message);
            return result;
        }

        public string GetMessage()
        {
            var result = ConvertHelper.Convert(Bytes);
            return result;
        }
    }
}