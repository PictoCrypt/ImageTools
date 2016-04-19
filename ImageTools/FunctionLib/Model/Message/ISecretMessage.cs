using FunctionLib.Cryptography;

namespace FunctionLib.Model.Message
{
    public interface ISecretMessage
    {
        CryptographicAlgorithmImpl Crypto { get; set; }
        string Password { set; }
        bool Compression { get; set; }
        string Message { get; }
        byte[] Bytes { get; }
        byte[] Convert();
        string ConvertBack();
    }
}