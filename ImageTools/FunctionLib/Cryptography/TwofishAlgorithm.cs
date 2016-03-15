using System.Security.Cryptography;

namespace FunctionLib.Cryptography
{
    public class TwofishAlgorithm : CryptographicAlgorithmImpl
    {
        protected override SymmetricAlgorithm Algorithm
        {
            get { return new Twofish.Twofish(); }
        }
    }
}