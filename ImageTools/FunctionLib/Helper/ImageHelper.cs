using System;
using System.Drawing;
using System.Linq;

namespace FunctionLib.Helper
{
    public static class ImageHelper
    {
        public static bool TransparentOrWhite(Color pixel, int lsbIndicator)
        {
            return ContainsTransparent(pixel) || ContainsWhite(pixel) || CheckIfWhiteAfterEncoding(pixel, lsbIndicator);
        }

        private static bool CheckIfWhiteAfterEncoding(Color pixel, int lsbIndicator)
        {
            var maxFreq = ByteHelper.ClearLeastSignificantBit(255, lsbIndicator);
            var max = MathHelper.Max(new[] { pixel.R, pixel.G, pixel.B }.Select(Convert.ToInt32).ToArray());
            return max > maxFreq;
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