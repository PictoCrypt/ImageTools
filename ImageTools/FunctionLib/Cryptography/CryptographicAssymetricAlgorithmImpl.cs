using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace FunctionLib.Cryptography
{
    public abstract class CryptographicAssymetricAlgorithmImpl : CryptographicAlgorithmImpl
    {
        protected abstract int KeySize { get; }
        protected abstract RSACryptoServiceProvider Algorithm { get; }

        public string[] GenerateKeys()
        {
            var algorithm = Algorithm;
            var privKey = algorithm.ExportParameters(true);
            var pubKey = algorithm.ExportParameters(false);
            return new[] {KeyToString(pubKey), KeyToString(privKey)};
        }

        public string KeyToString(RSAParameters parameter)
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
            //IEnumerable<byte> encoded = new byte[] {};
            byte[] encoded;
            using (var cipher = Algorithm)
            {
                cipher.ImportParameters(StringToKey(password));
                encoded = cipher.Encrypt(value, false);
                //var i = 0;
                //var x = ((KeySize - 384)/8) + 37;
                //while (value.Length > 0)
                //{
                //    var current = value.Skip(i++ * x).Take(x).ToArray();
                //    value = value.Skip(x).ToArray();
                //    encoded = encoded.Concat(cipher.Encrypt(current, false));
                //}
            }
            return encoded;
            //return encoded.ToArray();
        }

        public override byte[] Decode(byte[] value, string password)
        {
            //IEnumerable<byte> decoded = new byte[] {};
            byte[] decoded;
            using (var cipher = Algorithm)
            {
                cipher.ImportParameters(StringToKey(password));
                decoded = cipher.Decrypt(value, false);
                //var i = 0;
                //var x = ((KeySize - 384)/8) + 37;
                //while (value.Length > 0)
                //{
                //    var current = value.Skip(i++ * x).Take(x).ToArray();
                //    value = value.Skip(x).ToArray();
                //    decoded = decoded.Concat(cipher.Decrypt(current, false));
                //}
            }
            return decoded;
            //return decoded.ToArray();
        }
    }
}