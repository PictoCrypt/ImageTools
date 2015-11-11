﻿using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using FunctionLib.Cryptography.Blowfish;
using FunctionLib.Enums;

namespace FunctionLib.Cryptography
{
    public static class SymmetricAlgorithmBase
    {
        private const int Iterations = 2;
        private const string Salt = "jasdh7834y8hfeur73rsharks214"; // Random
        private const string Vector = "8947az34awl34kjq"; // Random
        private static int mKeySize = 256;

        public static string Encrypt(object obj, EncryptionMethod method, string value, string password)
        {
            var encryptionType = MethodNameToType(method);
            var baseType = typeof (SymmetricAlgorithmBase);
            var extractedMethod = baseType.GetMethods().FirstOrDefault(x => x.IsGenericMethod && x.Name == "Encrypt");
            if (extractedMethod != null)
            {
                return extractedMethod.MakeGenericMethod(encryptionType)
                    .Invoke(obj, new object[] {value, password}).ToString();
            }
            throw new ArgumentException(baseType.ToString());
        }

        public static string Decrypt(object obj, EncryptionMethod method, string value, string password)
        {
            var encryptionType = MethodNameToType(method);
            var baseType = typeof (SymmetricAlgorithmBase);
            var extractedMethod = baseType.GetMethods().FirstOrDefault(x => x.IsGenericMethod && x.Name == "Decrypt");
            if (extractedMethod != null)
            {
                return extractedMethod.MakeGenericMethod(encryptionType)
                    .Invoke(obj, new object[] {value, password}).ToString();
            }
            throw new ArgumentException(baseType.ToString());
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
            if (typeof (T) == typeof (Twofish.Twofish))
            {
                var index = result.IndexOf("\0", StringComparison.Ordinal);
                return result.Remove(index, result.Length - index);
            }
            return result;
        }

        private static Type MethodNameToType(EncryptionMethod method)
        {
            switch (method)
            {
                case EncryptionMethod.AES:
                    return typeof (AesCryptoServiceProvider);
                case EncryptionMethod.DES:
                    return typeof (DESCryptoServiceProvider);
                case EncryptionMethod.RC2:
                    return typeof (RC2CryptoServiceProvider);
                case EncryptionMethod.Rijndael:
                    return typeof (RijndaelManaged);
                case EncryptionMethod.TripleDES:
                    return typeof (TripleDESCryptoServiceProvider);
                case EncryptionMethod.Twofish:
                    return typeof (Twofish.Twofish);
                case EncryptionMethod.Blowfish:
                    return typeof (BlowfishAlgorithm);
            }
            throw new ArgumentOutOfRangeException(nameof(method), method, null);
        }
    }
}