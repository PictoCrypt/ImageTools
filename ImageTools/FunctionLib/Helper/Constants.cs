using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FunctionLib.Helper
{
    public static class Constants
    {
        public static string ExecutiongPath
        {
            get
            {
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                return path;
            }
        }

        public static string TempImagePath
        {
            get
            {
                return TempFilePathWithoutExtension + ".png";
            }
        }

        public static string TempFilePathWithoutExtension
        {
            get { return Path.GetTempPath() + Guid.NewGuid(); }
        }

        public static string Encoding
        {
            get { return "ISO-8859-1"; }
        }

        public static readonly List<string> ImageExtensions = new List<string> { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };
        public static readonly byte[] EndOfFileBytes = ConvertHelper.ToByteArray("<EOF>");

        public static byte[] StartOfFileBytes(string type)
        {
            var value = string.Format("<{0}>", type.ToUpperInvariant());
            var result = ConvertHelper.ToByteArray(value);
            return result;
        }

        public static string TempFilePath(string extension)
        {
            var result = TempFilePathWithoutExtension + extension;
            return result;
        }
    }
}