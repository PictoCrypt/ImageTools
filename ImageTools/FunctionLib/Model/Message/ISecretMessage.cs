namespace FunctionLib.Model.Message
{
    public interface ISecretMessage
    {
        bool Compression { get; set; }
        string Message { get; }
        byte[] Bytes { get; }
        byte[] Convert();
        string ConvertBack();
    }
}