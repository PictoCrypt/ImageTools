using System.IO;
using System.IO.Compression;
using FunctionLib.Helper;

namespace FunctionLib.Model.Message
{
    public class DocumentMessage : SecretMessage, ISecretMessage
    {
        public byte[] Convert()
        {
            if (!File.Exists(Message))
            {
                throw new FileNotFoundException(Message);
            }

            var memStream = new MemoryStream();
            using (var fs = new FileStream(Message, FileMode.Open))
            {
                fs.CopyTo(memStream);
            }

            byte[] result;
            using (var ms = new MemoryStream(memStream.ToArray()))
            {
                result = CompressionHelper.Compress(ms, CompressionLevel);
            }
            return result;
        }

        public object ConvertBack()
        {
            var path = FileManager.GetInstance().GenerateTmp();
            var ms = CompressionHelper.Decompress(Bytes, CompressionLevel);
            using (var fs = File.Create(path))
            {
                ms.CopyTo(fs);
                fs.Flush();
            }
            return path;
        }

        public DocumentMessage(string obj, CompressionLevel compression = CompressionLevel.NoCompression) : base(obj, compression)
        {
        }

        public DocumentMessage(byte[] bytes, CompressionLevel compression = CompressionLevel.NoCompression) : base(bytes, compression)
        {
        }
    }
}