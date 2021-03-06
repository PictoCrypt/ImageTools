﻿using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using FunctionLib.Helper;
using FunctionLib.Model.Message;

namespace FunctionLib.Steganography
{
    public class AppendMessageAlgorithm : SteganographicAlgorithmImpl
    {
        public override string Name
        {
            get { return "Append Message Algorithm"; }
        }

        public override string Description
        {
            get { return "Appending the secret message at the end of the image."; }
        }

        public override IList<ImageFormat> PossibleImageFormats
        {
            get
            {
                var result = Constants.ImageFormats;
                return result;
            }
        }

        protected override bool IsEncryptionPossible()
        {
            return true;
        }

        protected override string EncodingAlgorithm(string src, ISecretMessage message)
        {
            var data = message.Convert();
            var file = FileManager.CopyImageToTmp(src);
            using (var sw = new StreamWriter(File.Open(file, FileMode.Append)))
            {
                sw.Write("\n");
                sw.Write("\n");
                sw.Write("\n");
                sw.Write(ConvertHelper.Convert(data));
            }
            return file;
        }

        protected override ISecretMessage DecodingAlgorithm(string src, int lsbIndicator)
        {
            var text = string.Empty;
            using (var sr = new StreamReader(File.OpenRead(src)))
            {
                text = sr.ReadToEnd();
            }
            var index = text.LastIndexOf("\n\n");
            var result = string.Empty;
            var seq = text.Skip(index + 2);
            result = seq.Aggregate(result, (current, c) => current + c);
            var sizeIndex = result.IndexOf(Constants.Seperator);
            result = result.Remove(0, sizeIndex + 1);

            return GetSpecificMessage(ConvertHelper.Convert(result));
        }

        public override int MaxEmbeddingCount(Bitmap src, int lsbIndicator)
        {
            return int.MaxValue;
        }
    }
}