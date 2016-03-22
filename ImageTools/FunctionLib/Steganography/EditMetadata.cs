using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using FunctionLib.Helper;
using FunctionLib.Model.Message;

namespace FunctionLib.Steganography
{
    public class EditMetadata : SteganographicAlgorithmImpl
    {
        public override string Name { get { return "Edit Metadata"; } }
        public override string Description { get { return "Embedding secret message into the metadata of an image."; } }
        public override string Encode(string src, ISecretMessage message, int passHash, int lsbIndicator = 3)
        {
            var tmp = FileManager.CopyImageToTmp(src);
            using (var bmp = new Bitmap(src))
            {
                var item = bmp.PropertyItems.OrderByDescending(x => x.Id).First();
                var data = message.Convert();
                item.Id = item.Id + 1;
                item.Len = data.Length;
                item.Value = data;
                item.Type = 1;
                bmp.SetPropertyItem(item);
                bmp.Save(tmp);
            }
            return tmp;
        }

        public override ISecretMessage Decode(string src, int passHash, int lsbIndicator = 3)
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

        public override IList<ImageFormat> PossibleImageFormats
        {
            get { return Constants.ImageFormats; }
        }

        public override int MaxEmbeddingCount(Bitmap src, int lsbIndicator)
        {
            return (int) Math.Pow(2, 16);
        }
    }
}