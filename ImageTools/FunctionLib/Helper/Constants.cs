using System;
using System.IO;
using System.Reflection;

namespace FunctionLib.Helper
{
    public class Constants
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
                return Path.GetTempPath() + Guid.NewGuid() + ".png";
            }
        }
    }
}