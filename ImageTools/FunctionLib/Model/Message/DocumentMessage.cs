using System.IO;
using System.Linq;
using FunctionLib.Helper;

namespace FunctionLib.Model.Message
{
    public class DocumentMessage : SecretMessage, ISecretMessage
    {
        public DocumentMessage(string path, bool compression = true)
            : base(path, compression)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(path);
            }
        }

        public DocumentMessage(byte[] bytes, bool compression = true)
            : base(bytes, compression)
        {
        }


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

            var result = memStream.ToArray();
            if (Compression)
            {
                using (var ms = new MemoryStream(memStream.ToArray()))
                {
                    result = CompressionHelper.Compress(ms);
                }
            }

            var length = ConvertHelper.Convert(result.Length.ToString());
            var path = ConvertHelper.Convert(Path.GetExtension(Message));
            result =
                length.Concat(Constants.TagSeperator)
                    .Concat(path)
                    .Concat(Constants.TagSeperator)
                    .Concat(result)
                    .ToArray();
            return result;
        }

        public string ConvertBack()
        {
            var index = ListHelper.IndexOf(Bytes, Constants.TagSeperator);
            var sep1 = Bytes.Skip(index + Constants.TagSeperator.Length).ToArray();
            var extension = ConvertHelper.Convert(sep1.Take(ListHelper.IndexOf(sep1, Constants.TagSeperator)).ToArray());
            var resulting = sep1.Skip(ListHelper.IndexOf(Bytes, Constants.TagSeperator)).ToArray();

            var path = FileManager.GetInstance().GenerateTmp(extension);
            var ms = Compression ? CompressionHelper.Decompress(resulting) : new MemoryStream(resulting);
            using (var fs = File.Create(path))
            {
                ms.CopyTo(fs);
                fs.Flush();
            }
            return path;
        }
    }
}