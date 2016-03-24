using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace FunctionLib.Helper
{
    public static class ImageHelper
    {
        public static bool TransparentOrWhite(Color pixel)
        {
            return ContainsTransparent(pixel) || ContainsWhite(pixel);
        }

        //TODO Transparenz beginnt nicht erst ab 255 und niedriger sowie Weiß nicht erst bei FFFFFF
        private static bool ContainsTransparent(Color pixel)
        {
            var result = pixel.A != 255;
            return result;
        }

        private static bool ContainsWhite(Color pixel)
        {
            var result = pixel.Name.Equals("ffffffff", StringComparison.OrdinalIgnoreCase);
            return result;
        }
    }
}