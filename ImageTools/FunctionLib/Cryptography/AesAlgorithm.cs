using System.Security.Cryptography;

namespace FunctionLib.Cryptography
{
    public class AesAlgorithm : CryptographicAlgorithmImpl
    {
        protected override SymmetricAlgorithm Algorithm
        {
            get { return new AesCryptoServiceProvider(); }
        }
    }
}