using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Text;

namespace FunctionLib.Helper
{
    public static class ConvertHelper
    {
        private static Encoding Encoder
        {
            get { return Encoding.GetEncoding(Constants.Encoding); }
        }

        public static string GenerateFilter(IList<ImageFormat> formats)
        {
            var seperator = "|";
            var builder = new StringBuilder();
            foreach (var format in formats)
            {
                if (builder.Length != 0)
                {
                    builder.Append(seperator);
                }
                builder.Append(string.Format("{0}{1}*.{2}", format.ToString().ToUpperInvariant(), seperator, format.ToString().ToLowerInvariant()));
            }

            return builder.ToString();
        }

        public static byte[] Convert(string value)
        {
            var result = Encoder.GetBytes(value);
            return result;
        }

        public static string Convert(byte[] value)
        {
            var result = Encoder.GetString(value);
            return result;
        }
    }
}