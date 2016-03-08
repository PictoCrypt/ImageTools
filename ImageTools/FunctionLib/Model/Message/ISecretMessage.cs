using System.IO.Compression;

namespace FunctionLib.Model.Message
{
    public interface ISecretMessage
    {
        bool Compression { get; }
        string Message { get; }
        byte[] Bytes { get; }
        byte[] Convert();
        string ConvertBack();
    }
}