using System;
using System.Collections.Generic;
using System.Drawing;

namespace FunctionLib.Model
{
    public class Steganogramm
    {
        public Bitmap SrcBitmap { get; set; }
        public Bitmap StegoBitmap { get; set; }
        public List<Pixel> ChangedPixels { get; set; }

        public Bitmap GetSimulation()
        {
            throw new NotImplementedException();
        }
    }
}