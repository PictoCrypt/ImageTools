using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FunctionLib.Cryptography
{
    public static class SymmetricAlgorithmBase
    {
        private const int Iterations = 2;
        private const int KeySize = 256;
        private const string Salt = "jasdh7834y8hfeur73rsharks214"; // Random
        private const string Vector = "8947az34awl34kjq"; // Random

        public static string Encrypt(string value, string password)
        {
            return Encrypt<AesManaged>(value, password);
        }

        public static string Decrypt(string value, string password)
        {
            return Decrypt<AesManaged>(value, password);
        }

        public static string Encrypt<T>(string value, string password)
            where T : SymmetricAlgorithm, new()
        {
            var vectorBytes = Encoding.ASCII.GetBytes(Vector);
            var saltBytes = Encoding.ASCII.GetBytes(Salt);
            var valueBytes = Encoding.UTF8.GetBytes(value);
            byte[] encrypted;

            using (var cipher = new T())
            {
                var passwordBytes = new Rfc2898DeriveBytes(password, saltBytes, Iterations);
                var keyBytes = passwordBytes.GetBytes(KeySize / 8);

                cipher.Mode = CipherMode.CBC;

                using (var encryptor = cipher.CreateEncryptor(keyBytes, vectorBytes))
                {
                    using (var ms = new MemoryStream())
                    {
                        using (var writer = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            writer.Write(valueBytes, 0, valueBytes.Length);
                            writer.FlushFinalBlock();
                            encrypted = ms.ToArray();
                        }
                    }
                }
                cipher.Clear();
            }
            return Convert.ToBase64String(encrypted);
        }

        public static string Decrypt<T>(string value, string password) 
            where T : SymmetricAlgorithm, new()
        {
            var vectorBytes = Encoding.ASCII.GetBytes(Vector);
            var saltBytes = Encoding.ASCII.GetBytes(Salt);
            var valueBytes = Convert.FromBase64String(value);

            byte[] decrypted;
            int decryptedByteCount;

            using (var cipher = new T())
            {
                var passwordBytes = new Rfc2898DeriveBytes(password, saltBytes, Iterations);
                var keyBytes = passwordBytes.GetBytes(KeySize / 8);

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
            return Encoding.UTF8.GetString(decrypted, 0, decryptedByteCount);
        }
    }
}