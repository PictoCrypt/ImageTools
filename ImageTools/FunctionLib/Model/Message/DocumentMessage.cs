using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Linq;
using FunctionLib.Helper;

namespace FunctionLib.Model.Message
{
    public class DocumentMessage : SecretMessage, ISecretMessage
    {
        public DocumentMessage(string obj, CompressionLevel compression = CompressionLevel.NoCompression) 
            : base(obj, compression)
        {
            if (!File.Exists(obj))
            {
                throw new FileNotFoundException(obj);
            }
        }

        public DocumentMessage(byte[] bytes, CompressionLevel compression = CompressionLevel.NoCompression) 
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

            byte[] result;
            using (var ms = new MemoryStream(memStream.ToArray()))
            {
                result = CompressionHelper.Compress(ms, CompressionLevel);
            }

            var length = ConvertHelper.Convert(result.Length.ToString());
            var path = ConvertHelper.Convert(Path.GetExtension(Message));
            result = length.Concat(Constants.TagSeperator).Concat(path).Concat(Constants.TagSeperator).Concat(result).ToArray();
            return result;
        }

        public object ConvertBack()
        {
            var index = ListHelper.IndexOf(Bytes, Constants.TagSeperator);
            var sep1 = Bytes.Skip(index).ToArray();
            var extension = ConvertHelper.Convert(sep1.Take(index).ToArray());
            var resulting = sep1.Skip(ListHelper.IndexOf(Bytes, Constants.TagSeperator)).ToArray();

            var path = FileManager.GetInstance().GenerateTmp();
            var ms = CompressionHelper.Decompress(Bytes, CompressionLevel);
            using (var fs = File.Create(path))
            {
                ms.CopyTo(fs);
                fs.Flush();
            }
            return path;
        }
    }
}