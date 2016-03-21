using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using FunctionLib.Helper;

namespace FunctionLib.Model.Message
{
    public class DocumentMessage : SecretMessage, ISecretMessage
    {
        private readonly string mExtension;

        public DocumentMessage(string path, bool compression = false)
            : base(path, compression)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(path);
            }
        }

        public DocumentMessage(byte[] bytes, string extension, bool compression = false)
            : base(bytes, compression)
        {
            mExtension = extension;
        }


        public byte[] Convert()
        {
            if (!File.Exists(Message))
            {
                throw new FileNotFoundException(Message);
            }

            var memStream = new MemoryStream();
            if (Constants.ImageExtensions.Contains(Path.GetExtension(Message).Replace(".", ""),
                StringComparer.OrdinalIgnoreCase))
            {
                using (var img = Image.FromFile(Message))
                {
                    img.Save(memStream, img.RawFormat);
                }
            }
            else
            {
                using (var fs = new FileStream(Message, FileMode.Open))
                {
                    fs.CopyTo(memStream);
                }
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
            var path = ConvertHelper.Convert(Path.GetExtension(Message).Replace(".", ""));
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
            var path = FileManager.GetInstance().GenerateTmp(mExtension);
            var ms = Compression ? CompressionHelper.Decompress(Bytes) : new MemoryStream(Bytes);
            if (Constants.ImageExtensions.Contains(mExtension, StringComparer.OrdinalIgnoreCase))
            {
                using (var img = Image.FromStream(ms))
                {
                    img.Save(path);
                }
            }
            else
            {
                using (var fs = File.Create(path))
                {
                    ms.CopyTo(fs);
                    fs.Flush();
                }
            }
            return path;
        }
    }
}