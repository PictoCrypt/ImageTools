using System.Security.Cryptography;

namespace FunctionLib.Cryptography
{
    public class TwofishAlgorithm : CryptographicSymmetricAlgorithmImpl
    {
        protected override SymmetricAlgorithm Algorithm
        {
            get { return new Twofish.Twofish(); }
        }

        public override string Name
        {
            get { return "Twofish"; }
        }
    }
}