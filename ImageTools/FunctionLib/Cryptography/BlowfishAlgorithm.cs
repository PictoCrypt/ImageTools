using System.Security.Cryptography;

namespace FunctionLib.Cryptography
{
    public class BlowfishAlgorithm : CryptographicSymmetricAlgorithmImpl
    {
        protected override SymmetricAlgorithm Algorithm
        {
            get { return new Blowfish.Blowfish(); }
        }

        public override string Name
        {
            get { return "Blowfish"; }
        }
    }
}