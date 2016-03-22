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
        public override string Name { get { return "Edit Metadata"; } }
        public override string Description { get { return "Embedding secret message into the metadata of an image."; } }
        public override string Encode(string src, ISecretMessage message, int passHash, int lsbIndicator = 3)
        {
            var tmp = FileManager.CopyImageToTmp(src);
            using (var bmp = new Bitmap(tmp))
            {
                var item = bmp.PropertyItems.Last();
                item.Value = message.Convert();
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
                var item = bmp.PropertyItems.Last();
                result = item.Value;
            }
            return GetSpecificMessage(result);
        }

        public override IList<ImageFormat> PossibleImageFormats
        {
            get { return Constants.ImageFormats; }
        }
    }
}