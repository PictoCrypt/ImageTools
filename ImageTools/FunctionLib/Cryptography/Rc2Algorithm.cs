using System.Security.Cryptography;

namespace FunctionLib.Cryptography
{
    public class Rc2Algorithm : CryptographicSymmetricAlgorithmImpl
    {
        protected override SymmetricAlgorithm Algorithm
        {
            get { return new RC2CryptoServiceProvider(); }
        }

        public override string Name
        {
            get { return "RC2"; }
        }
    }
}