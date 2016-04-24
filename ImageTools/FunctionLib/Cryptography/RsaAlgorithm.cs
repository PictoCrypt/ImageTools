using System.Security.Cryptography;

namespace FunctionLib.Cryptography
{
    public class RsaAlgorithm : CryptographicAssymetricAlgorithmImpl
    {
        public override string Name { get { return "RSA"; } }
        protected override int KeySize { get { return 4096; } }
        protected override RSACryptoServiceProvider Algorithm { get { return new RSACryptoServiceProvider(4096); } }
    }
}