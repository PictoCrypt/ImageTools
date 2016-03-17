﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace FunctionLib.Helper
{
    public static class Constants
    {
        private const char Seperator = '\r';

        public const string ApplicationName = "ImageTools";
        public static readonly HashSet<string> ImageExtensions = new HashSet<string>{"JPG", "JPE", "BMP", "GIF", "PNG"};
        public const string ImageFilter =
            "Image Files (*.JPG, *.JPE, *.BMP, *.GIF, *.PNG)| *.jpg; *.jpe; *.bmp; *.gif; *.png";
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
    }
}