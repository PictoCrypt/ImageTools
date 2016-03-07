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

            var ms = new MemoryStream();
            using (var stream = new FileStream(Message, FileMode.Open))
            {
                stream.CopyTo(ms);
            }
            return CompressionHelper.Compress(ms, CompressionLevel);
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