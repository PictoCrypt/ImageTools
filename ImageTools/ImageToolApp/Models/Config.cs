using FunctionLib.Cryptography;
using FunctionLib.Steganography;

namespace ImageToolApp.Models
{
    public class Config
    {
        public Config(string defaultPath, string password, CryptographicAlgorithmImpl crypto,
            SteganographicAlgorithmImpl stego)
        {
            DefaultPath = defaultPath;
            Password = password;
            Crypto = crypto;
            Stego = stego;
        }

        public string DefaultPath { get; private set; }
        public string Password { get; private set; }
        public SteganographicAlgorithmImpl Stego { get; set; }
        public CryptographicAlgorithmImpl Crypto { get; set; }
    }
}