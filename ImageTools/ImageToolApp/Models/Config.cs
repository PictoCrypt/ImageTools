using System.Security.Cryptography;
using FunctionLib.Steganography.Base;

namespace ImageToolApp.Models
{
    public class Config
    {
        public Config(string defaultPath, string password, SymmetricAlgorithm crypto, SteganographicAlgorithmImpl stego)
        {
            DefaultPath = defaultPath;
            Password = password;
            Crypto = crypto;
            Stego = stego;
        }
        
        public string DefaultPath { get; private set; }
        public string  Password { get; private set; }
        public SteganographicAlgorithmImpl Stego { get; set; }
        public SymmetricAlgorithm Crypto { get; set; }
    }
}