using System;
using System.Drawing;
using System.IO;
using System.Linq;
using FunctionLib.Cryptography;
using FunctionLib.Helper;

namespace FunctionLib.Model.Message
{
    public class DocumentMessage : SecretMessage, ISecretMessage
    {
        private readonly string mExtension;

        public DocumentMessage(string path, bool compression = false, CryptographicAlgorithmImpl crypto = null,
            string password = null)
            : base(path, compression, crypto, password)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(path);
            }
        }

        public DocumentMessage(byte[] bytes, string extension, bool compression = false,
            CryptographicAlgorithmImpl crypto = null, string password = null)
            : base(bytes, compression, crypto, password)
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
            if (Crypto != null)
            {
                result = Crypto.Encode(result, Password);
            }

            if (Compression)
            {
                using (var ms = new MemoryStream(memStream.ToArray()))
                {
                    result = CompressionHelper.Compress(ms);
                }
            }

            var extension = ConvertHelper.Convert(Path.GetExtension(Message).Replace(".", ""));
            var listResult = extension.Concat(Constants.TagSeperator).Concat(result);
            var length = ConvertHelper.Convert(listResult.Count().ToString());
            listResult = length.Concat(Constants.TagSeperator).Concat(listResult);
            result = listResult.ToArray();
            return result;
        }

        public string ConvertBack()
        {
            var path = FileManager.GetInstance().GenerateTmp(mExtension);
            var ms = Compression ? CompressionHelper.Decompress(Bytes) : new MemoryStream(Bytes);

            if (Crypto != null)
            {
                ms = new MemoryStream(Crypto.Decode(ms.ToArray(), Password));
            }

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

        public override string Message
        {
            get { return mMessage; }
        }
    }
}