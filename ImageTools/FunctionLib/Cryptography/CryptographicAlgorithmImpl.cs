using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace FunctionLib.Cryptography
{
    public abstract class CryptographicAlgorithmImpl
    {
        private const int Iterations = 2;
        private const string Salt = "jasdh7834y8hfeur73rsharks214"; // Random
        private const string Vector = "8947az34awl34kjq"; // Random
        private static int mKeySize = 256;

        protected abstract SymmetricAlgorithm Algorithm { get; }

        public abstract string Name { get; }

        public string Encode(string value, string password)
        {
            var valueBytes = Encoding.UTF8.GetBytes(value);
            var result = Encode(valueBytes, password);
            return Convert.ToBase64String(result);
        }

        public byte[] Encode(byte[] value, string password)
        {
            var vectorBytes = Encoding.ASCII.GetBytes(Vector);
            var saltBytes = Encoding.ASCII.GetBytes(Salt);
            byte[] encoded;

            using (var cipher = Algorithm)
            {
                mKeySize = cipher.LegalKeySizes.Max().MaxSize;
                var passwordBytes = new Rfc2898DeriveBytes(password, saltBytes, Iterations);
                var keyBytes = passwordBytes.GetBytes(mKeySize / 8);

                cipher.Mode = CipherMode.CBC;

                using (var encryptor = cipher.CreateEncryptor(keyBytes, vectorBytes))
                {
                    using (var ms = new MemoryStream())
                    {
                        using (var writer = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            writer.Write(value, 0, value.Length);
                            writer.FlushFinalBlock();
                            encoded = ms.ToArray();
                        }
                    }
                }
                cipher.Clear();
            }
            return encoded;
        }

        public string Decode(string value, string password)
        {
            var valueBytes = Convert.FromBase64String(value);

            var bytes = Decode(valueBytes, password);
            var result = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            if (Algorithm is Twofish.Twofish)
            {
                var index = result.IndexOf("\0", StringComparison.Ordinal);
                return result.Remove(index, result.Length - index);
            }
            return result;
        }

        public byte[] Decode(byte[] value, string password)
        {
            var vectorBytes = Encoding.ASCII.GetBytes(Vector);
            var saltBytes = Encoding.ASCII.GetBytes(Salt);
            byte[] decrypted;

            using (var cipher = Algorithm)
            {
                var passwordBytes = new Rfc2898DeriveBytes(password, saltBytes, Iterations);
                var keyBytes = passwordBytes.GetBytes(mKeySize / 8);

                cipher.Mode = CipherMode.CBC;

                using (var decryptor = cipher.CreateDecryptor(keyBytes, vectorBytes))
                {
                    using (var ms = new MemoryStream(value))
                    {
                        using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                        {
                            cs.Write(value, 0, value.Length);
                            cs.Close();
                        }
                        decrypted = ms.ToArray();
                    }
                }
                cipher.Clear();
            }
            return decrypted;
        }

        public override string ToString()
        {
            return Name;
        }

        public override int GetHashCode()
        {
            var result = Name.GetHashCode() + Iterations.GetHashCode() + Salt.GetHashCode() + Vector.GetHashCode();
            return result;
        }

        public override bool Equals(object obj)
        {
            var other = obj as CryptographicAlgorithmImpl;
            if (other == null)
            {
                return false;
            }

            if (Name.Equals(other.Name))
            {
                return true;
            }
            return false;
        }
    }
}