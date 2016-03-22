using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;

namespace FunctionLib.Helper
{
    public static class Constants
    {
        public const char Seperator = '\r';

        public const string ApplicationName = "ImageTools";

        public static readonly HashSet<string> ImageExtensions = new HashSet<string> {"JPG", "JPE", "BMP", "GIF", "PNG"};
        public static readonly byte[] TagSeperator = ConvertHelper.Convert(Seperator.ToString());

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

        public static string AppData
        {
            get
            {
                var result = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    ApplicationName);
                if (!Directory.Exists(result))
                {
                    Directory.CreateDirectory(result);
                }
                return result;
            }
        }

        public static List<ImageFormat> ImageFormats
        {
            get
            {
                var t = new ImageFormat(new Guid());

                return new List<ImageFormat>
                {
                    ImageFormat.Png,
                    ImageFormat.Bmp,
                    ImageFormat.MemoryBmp,
                    ImageFormat.Gif,
                    ImageFormat.Jpeg,
                };
            }
        }
    }
}