using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using FunctionLib.Helper;

namespace FunctionLib.Model.Message
{
    public class TextMessage : SecretMessage, ISecretMessage
    {
        public TextMessage(string obj, CompressionLevel compression = CompressionLevel.NoCompression) : base(obj, compression)
        {
        }

        public TextMessage(byte[] bytes, CompressionLevel compression = CompressionLevel.NoCompression) : base(bytes, compression)
        {
        }

        public byte[] Convert()
        {
            byte[] result;
            using (var ms = new MemoryStream(ConvertHelper.StringToBytes(Message)))
            {
                result = CompressionHelper.Compress(ms, CompressionLevel);
            }
            return result;
        }

        public object ConvertBack()
        {
            var stream = CompressionHelper.Decompress(Bytes, CompressionLevel);
            string result;
            using (stream)
            {
                using (var reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }

            if (string.IsNullOrEmpty(result))
            {
                throw new IOException("Error reading from stream. Stream was empty.");
            }

            return result;
        }
    }
}