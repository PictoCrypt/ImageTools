using System.IO.Compression;

namespace FunctionLib.Model.Message
{
    public interface ISecretMessage
    {
        CompressionLevel CompressionLevel { get; }
        string Message { get; }
        byte[] Bytes { get; }
        byte[] Convert();
        object ConvertBack();
    }
}