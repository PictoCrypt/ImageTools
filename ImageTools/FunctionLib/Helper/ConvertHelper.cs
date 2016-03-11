using System.Text;

namespace FunctionLib.Helper
{
    public static class ConvertHelper
    {
        private static Encoding Encoder
        {
            get { return Encoding.GetEncoding(Constants.Encoding); }
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

        /// </summary>
        ///     Converts a object into a byte-Array. Adds an Start and End-Tag to the object.

        /// <summary>
        /// <param name="value">The object can be a string or an path to an file.</param>
        /// <returns>byte[]</returns>
        //public static byte[] Convert(string value)
        //{
        //    var str = value;
        //    if (value == null || string.IsNullOrEmpty(str))
        //    {
        //        throw new ArgumentException("value is null or empty.");
        //    }

        //    var bytes = AddStartTag(str);

        //    bytes = bytes.Concat(AddContent(str));

        //    if (!bytes.Any())
        //    {
        //        throw new ArgumentException("Cant cast object to anything which contains byte[] for me, tho.");
        //    }

        //    bytes = bytes.Concat(Constants.EndTag);
        //    return bytes.ToArray();
        //}

        //private static IEnumerable<byte> AddContent(string value)
        //{
        //    byte[] result;
        //    if (!File.Exists(value))
        //    {
        //        using (var stream = StringToStream(value))
        //        {
        //            result = CompressionHelper.Compress(stream);
        //        }
        //    }
        //    else
        //    {
        //        using (var fileStream = File.Open(value, FileMode.Open))
        //        {
        //            result = CompressionHelper.Compress(fileStream);
        //        }
        //    }
        //    return result;
        //}

        //private static IEnumerable<byte> AddStartTag(string value)
        //{
        //    if (!File.Exists(value))
        //    {
        //        return Constants.StartTag("Text");
        //    }
        //    var extension = Path.GetExtension(value).ToUpperInvariant();
        //    return Constants.StartTag(Constants.ImageExtensions.Contains(extension) ? "Image" : extension);
        //}

        //private static Stream StringToStream(string value)
        //{
        //    var bytes = Convert(value);
        //    return new MemoryStream(bytes);
        //}

        //private static string StreamToString(Stream stream)
        //{
        //    string result;
        //    using (var reader = new StreamReader(stream, Encoder))
        //    {
        //        reader.BaseStream.Seek(0, SeekOrigin.Begin);
        //        result = reader.ReadToEnd();
        //    }
        //    return result;
        //}

        //public static string Convert(byte[] bytes)
        //{
        //    string result;
        //    if (bytes == null || bytes.Length <= 0)
        //    {
        //        throw new ArgumentException("bytes is null or empty.");
        //    }
        //    var byteList = bytes.ToList();

        //    RemoveEndTag(byteList);

        //    var type = GetStartTag(byteList);
        //    using (var uncompressed = CompressionHelper.Decompress(byteList.ToArray()))
        //    {
        //        switch (type)
        //        {
        //            case "TEXT":
        //                result = StreamToString(uncompressed);
        //                break;
        //            case "IMAGE":
        //                result = FileManager.GetInstance().GenerateTmp(ImageFormat.Png);
        //                var returnImage = Image.FromStream(uncompressed);
        //                returnImage.Save(result);
        //                break;
        //            default:
        //                if (!type.StartsWith("."))
        //                {
        //                    throw new NotSupportedException("Invalid Start-Tag.");
        //                }

        //                result = FileManager.GetInstance().GenerateTmp(type);
        //                using (var fs = File.Create(result))
        //                {
        //                    uncompressed.CopyTo(fs);
        //                    fs.Flush();
        //                }
        //                break;
        //        }
        //    }
        //    return result;
        //}

        //private static string GetStartTag(List<byte> byteList)
        //{
        //    var index = byteList.IndexOf(Convert(">").First());
        //    if (index == -1)
        //    {
        //        throw new ArgumentException("Start-Tag not found.");
        //    }

        //    var range = byteList.GetRange(0, index + 1);
        //    range.RemoveAt(0);
        //    range.RemoveAt(range.Count - 1);
        //    byteList.RemoveRange(0, index + 1);

        //    var type = Convert(range.ToArray()).ToUpperInvariant();
        //    return type;
        //}

        //private static void RemoveEndTag(List<byte> byteList)
        //{
        //    // Remove EndTag
        //    byteList.RemoveRange(byteList.Count - Constants.EndTag.Length, Constants.EndTag.Length);
        //}
    }
}