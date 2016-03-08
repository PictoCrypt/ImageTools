using System.IO;
using System.IO.Compression;
using System.Linq;
using FunctionLib.Helper;

namespace FunctionLib.Model.Message
{
    public class TextMessage : SecretMessage, ISecretMessage
    {
        public TextMessage(string obj, bool compression = false) 
            : base(obj, compression)
        {
        }

        public TextMessage(byte[] bytes, bool compression = false) 
            : base(bytes, compression)
        {
        }

        public byte[] Convert()
        {
            byte[] result;
            if (Compression)
            {
                using (var ms = new MemoryStream(ConvertHelper.Convert(Message)))
                {
                    result = CompressionHelper.Compress(ms);
                }
            }
            else
            {
                result = ConvertHelper.Convert(Message);
            }

            var length = ConvertHelper.Convert(result.Length.ToString());
            result = length.Concat(Constants.TagSeperator).Concat(result).ToArray();
            return result;
        }


        public string ConvertBack()
        {
            var index = ListHelper.IndexOf(Bytes, Constants.TagSeperator);
            var bytes = Bytes.Skip(index + Constants.TagSeperator.Length).ToArray();
            if (Compression)
            {
                bytes = CompressionHelper.Decompress(bytes).ToArray();
            }

            var result = ConvertHelper.Convert(bytes.ToArray());
            
            if (string.IsNullOrEmpty(result))
            {
                throw new IOException("Error reading from stream. Stream was empty.");
            }
            return result;
        }
    }
}