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
            var i = -1;
            var seperator = "|";
            var builder = new StringBuilder();

            builder.Append("All Formats");
            foreach (var format in formats)
            {
                i++;
                if (i == 0)
                {
                    builder.Append(seperator);
                    if (format.Equals(ImageFormat.Jpeg))
                    {
                        builder.AppendFormat("*.{0}", "jpg");
                    }
                    else
                    {
                        builder.AppendFormat("*.{0}", format.ToString().ToLowerInvariant());
                    }
                }
                else
                {
                    if (format.Equals(ImageFormat.Jpeg))
                    {
                        builder.AppendFormat("{0}*.{1}", ";", "jpg");
                    }
                    else
                    {
                        builder.AppendFormat("{0}*.{1}", ";", format.ToString().ToLowerInvariant());
                    }
                }
            }

            foreach (var format in formats)
            {
                builder.Append(seperator);
                if (format.Equals(ImageFormat.Jpeg))
                {
                    builder.AppendFormat("{0}{1}*.{2}", format.ToString().ToUpperInvariant(), seperator, "jpg");
                }
                else
                {
                    builder.AppendFormat("{0}{1}*.{2}", format.ToString().ToUpperInvariant(), seperator, format.ToString().ToLowerInvariant());
                }
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