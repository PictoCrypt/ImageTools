using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace FunctionLib.Cryptography
{
    public abstract class CryptographicAssymetricAlgorithmImpl : CryptographicAlgorithmImpl
    {
        protected abstract RSACryptoServiceProvider Algorithm { get; }

        public string[] GenerateKeys()
        {
            var algorithm = Algorithm;
            var privKey = algorithm.ExportParameters(true);
            var pubKey = algorithm.ExportParameters(false);
            return new[] { KeyToString(pubKey), KeyToString(privKey)};
        }

        private string KeyToString(RSAParameters parameter)
        {
            string result;
            using (var sw = new StringWriter())
            {
                var xs = new XmlSerializer(typeof(RSAParameters));
                xs.Serialize(sw, parameter);
                result = sw.ToString();
            }
            return result;
        }

        private RSAParameters StringToKey(string parameter)
        {
            RSAParameters result;
            using (var sw = new StringReader(parameter))
            {
                var xs = new XmlSerializer(typeof(RSAParameters));
                result = (RSAParameters) xs.Deserialize(sw);
            }
            return result;
        }

        public override byte[] Encode(byte[] value, string password)
        {
            byte[] encoded;
            using (var cipher = Algorithm)
            {
                cipher.ImportParameters(StringToKey(password));
                encoded = cipher.Encrypt(value, false);
            }
            return encoded;
        }

        public override byte[] Decode(byte[] value, string password)
        {
            byte[] decoded;
            using (var cipher = Algorithm)
            {
                cipher.ImportParameters(StringToKey(password));
                decoded = cipher.Decrypt(value, false);
            }
            return decoded;
        }
    }
}