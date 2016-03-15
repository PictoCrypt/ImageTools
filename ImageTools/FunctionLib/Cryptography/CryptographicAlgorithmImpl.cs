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

        protected abstract string Name { get; }

        public string Encode(string value, string password)
        {
            var vectorBytes = Encoding.ASCII.GetBytes(Vector);
            var saltBytes = Encoding.ASCII.GetBytes(Salt);
            var valueBytes = Encoding.UTF8.GetBytes(value);
            byte[] encoded;

            using (var cipher = Algorithm)
            {
                mKeySize = cipher.LegalKeySizes.Max().MaxSize;
                var passwordBytes = new Rfc2898DeriveBytes(password, saltBytes, Iterations);
                var keyBytes = passwordBytes.GetBytes(mKeySize/8);

                cipher.Mode = CipherMode.CBC;

                using (var encryptor = cipher.CreateEncryptor(keyBytes, vectorBytes))
                {
                    using (var ms = new MemoryStream())
                    {
                        using (var writer = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            writer.Write(valueBytes, 0, valueBytes.Length);
                            writer.FlushFinalBlock();
                            encoded = ms.ToArray();
                        }
                    }
                }
                cipher.Clear();
            }
            return Convert.ToBase64String(encoded);
        }

        public string Decode(string value, string password)
        {
            var vectorBytes = Encoding.ASCII.GetBytes(Vector);
            var saltBytes = Encoding.ASCII.GetBytes(Salt);
            var valueBytes = Convert.FromBase64String(value);

            byte[] decrypted;
            int decryptedByteCount;

            using (var cipher = Algorithm)
            {
                var passwordBytes = new Rfc2898DeriveBytes(password, saltBytes, Iterations);
                var keyBytes = passwordBytes.GetBytes(mKeySize/8);

                cipher.Mode = CipherMode.CBC;

                using (var decryptor = cipher.CreateDecryptor(keyBytes, vectorBytes))
                {
                    using (var from = new MemoryStream(valueBytes))
                    {
                        using (var reader = new CryptoStream(@from, decryptor, CryptoStreamMode.Read))
                        {
                            decrypted = new byte[valueBytes.Length];
                            try
                            {
                                decryptedByteCount = reader.Read(decrypted, 0, decrypted.Length);
                            }
                            catch (Exception)
                            {
                                throw new SystemException("Das angegebene Passwort war falsch.");
                            }
                        }
                    }
                }

                cipher.Clear();
            }

            var result = Encoding.UTF8.GetString(decrypted, 0, decryptedByteCount);
            if (Algorithm is Twofish.Twofish)
            {
                var index = result.IndexOf("\0", StringComparison.Ordinal);
                return result.Remove(index, result.Length - index);
            }
            return result;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}