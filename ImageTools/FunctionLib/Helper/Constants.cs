using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace FunctionLib.Helper
{
    public static class Constants
    {
        public static readonly List<string> ImageExtensions = new List<string> {".JPG", ".JPE", ".BMP", ".GIF", ".PNG"};
        public static readonly byte[] EndTag = ConvertHelper.StringToBytes("<EOF>");

        public static string ExecutiongPath
        {
            get
            {
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                return path;
            }
        }

        public static string Encoding
        {
            get { return "ISO-8859-1"; }
        }

        public static byte[] StartTag(string type)
        {
            var value = string.Format("<{0}>", type.ToUpperInvariant());
            var result = ConvertHelper.StringToBytes(value);
            return result;
        }
    }
}