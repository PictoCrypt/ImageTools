using System.Security.Cryptography;

namespace FunctionLib.Cryptography
{
    public class TripleDesAlgorithm : CryptographicAlgorithmImpl
    {
        protected override SymmetricAlgorithm Algorithm
        {
            get { return new TripleDESCryptoServiceProvider(); }
        }

        public override string Name
        {
            get { return "TripleDES"; }
        }
    }
}