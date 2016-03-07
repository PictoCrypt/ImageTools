using System.IO.Compression;

namespace FunctionLib.Model.Message
{
    public interface ISecretMessage
    {
        CompressionLevel CompressionLevel { get; }
        string Message { get; set; }
        byte[] Bytes { get; set; }
        byte[] Convert();
        object ConvertBack();
    }
}