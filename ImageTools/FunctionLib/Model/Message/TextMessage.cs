using System.IO;
using System.IO.Compression;
using System.Linq;
using FunctionLib.Helper;

namespace FunctionLib.Model.Message
{
    public class TextMessage : SecretMessage, ISecretMessage
    {
        public TextMessage(string obj, CompressionLevel compression = CompressionLevel.NoCompression) 
            : base(obj, compression)
        {
        }

        public TextMessage(byte[] bytes, CompressionLevel compression = CompressionLevel.NoCompression) 
            : base(bytes, compression)
        {
        }

        public byte[] Convert()
        {
            byte[] result;
            using (var ms = new MemoryStream(ConvertHelper.Convert(Message)))
            {
                result = CompressionHelper.Compress(ms, CompressionLevel);
            }

            var length = ConvertHelper.Convert(result.Length.ToString());
            result = length.Concat(Constants.TagSeperator).Concat(result).ToArray();
            return result;
        }


        public object ConvertBack()
        {
            var index = ListHelper.IndexOf(Bytes, Constants.TagSeperator);
            var resulting = Bytes.Skip(index).ToArray();
            var stream = CompressionHelper.Decompress(resulting, CompressionLevel);
            string result;
            using (stream)
            {
                result = ConvertHelper.Convert(stream.ToArray());
            }
            
            if (string.IsNullOrEmpty(result))
            {
                throw new IOException("Error reading from stream. Stream was empty.");
            }
            return result;
        }
    }
}