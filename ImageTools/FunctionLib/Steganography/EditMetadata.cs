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
        public override Bitmap Encode(Bitmap src, ISecretMessage message, int passHash, int lsbIndicator = 3)
        {
            var tmp = FileManager.GetInstance().CopyImageToTmp(src);
            src = new Bitmap(tmp);
            var item = src.PropertyItems.Last();
            item.Value = message.Convert();
            src.SetPropertyItem(item);
            return src;
        }

        public override ISecretMessage Decode(Bitmap src, int passHash, int lsbIndicator = 3)
        {
            byte[] result;
            using (src)
            {
                var item = src.PropertyItems.Last();
                result = item.Value;
            }
            return GetSpecificMessage(result);
        }

        public override IList<ImageFormat> PossibleImageFormats
        {
            get { return Enum.GetValues(typeof (ImageFormat)).Cast<ImageFormat>().ToList(); }
        }
    }
}