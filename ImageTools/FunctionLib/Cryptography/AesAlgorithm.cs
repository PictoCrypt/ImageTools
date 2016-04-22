using System.Security.Cryptography;

namespace FunctionLib.Cryptography
{
    public class AesAlgorithm : CryptographicSymmetricAlgorithmImpl
    {
        protected override SymmetricAlgorithm Algorithm
        {
            get { return new AesCryptoServiceProvider(); }
        }

        public override string Name
        {
            get { return "AES"; }
        }
    }
}