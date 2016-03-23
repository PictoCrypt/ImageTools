using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using FunctionLib.Helper;
using FunctionLib.Model.Message;

namespace FunctionLib.Steganography
{
    public class EditMetadata : SteganographicAlgorithmImpl
    {
        public override string Name
        {
            get { return "Edit Metadata"; }
        }

        public override string Description
        {
            get { return "Embedding secret message into the metadata of an image."; }
        }

        private byte[] Bytes { get; set; }

        public override IList<ImageFormat> PossibleImageFormats
        {
            get { return new List<ImageFormat> {ImageFormat.Jpeg}; }
        }

        protected override bool IsEncryptionPossible()
        {
            return !(Bytes.Length >= Math.Pow(2, 16));
        }

        protected override void InitializeEncoding(string src, ISecretMessage message, int passHash, int lsbIndicator)
        {
            base.InitializeEncoding(src, message, passHash, lsbIndicator);
            Bytes = message.Convert();
        }

        protected override string EncodingAlgorithm(string src, ISecretMessage message, int passHash,
            int lsbIndicator)
        {
            var tmp = FileManager.CopyImageToTmp(src);
            using (var bmp = new Bitmap(src))
            {
                var item = bmp.PropertyItems.OrderByDescending(x => x.Id).First();
                item.Id = item.Id + 1;
                item.Len = Bytes.Length;
                item.Value = Bytes;
                item.Type = 1;
                bmp.SetPropertyItem(item);
                bmp.Save(tmp);
            }
            return tmp;
        }

        protected override ISecretMessage DecodingAlgorithm(string src, int passHash, int lsbIndicator)
        {
            byte[] result;
            using (var bmp = new Bitmap(src))
            {
                var item = bmp.PropertyItems.OrderByDescending(x => x.Id).First();
                result = item.Value;
            }

            result = RemoveSizeTag(result);
            return GetSpecificMessage(result);
        }

        private byte[] RemoveSizeTag(byte[] bytes)
        {
            var index = ListHelper.IndexOf(bytes, Constants.TagSeperator);
            var result = bytes.Skip(index + 1).ToArray();
            return result;
        }

        public override int MaxEmbeddingCount(Bitmap src, int lsbIndicator)
        {
            return (int) Math.Pow(2, 16);
        }
    }
}