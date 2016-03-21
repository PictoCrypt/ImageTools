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
    public class AppendMessageAlgorithm : SteganographicAlgorithmImpl
    {
        public override string Name { get { return "Append Message Algorithm"; } }
        public override string Description { get { return "Appending the secret message at the end of the image."; } }
        public override Bitmap Encode(Bitmap src, ISecretMessage message, int passHash, int lsbIndicator = 3)
        {
            //TODO We cant return an filename. --

            var fileManager = FileManager.GetInstance();
            var path = fileManager.CopyImageToTmp(src);
            File.AppendAllText(path, message.Message);
            return new Bitmap(path);
        }

        public override ISecretMessage Decode(Bitmap src, int passHash, int lsbIndicator = 3)
        {
            //TODO we need a filename
            using (var reader = new StreamReader(File.OpenRead("")))
            {
                reader.ReadToEnd();
            }

            throw new System.NotImplementedException();
        }

        public override IList<ImageFormat> PossibleImageFormats
        {
            get { return Enum.GetValues(typeof (ImageFormat)).Cast<ImageFormat>().ToList(); }
        }
    }
}