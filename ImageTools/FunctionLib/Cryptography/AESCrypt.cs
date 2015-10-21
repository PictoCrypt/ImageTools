using System;
using System.IO;
using System.Security.Cryptography;

namespace FunctionLib.Cryptography
{
    public class AESCrypt : Crypt
    {
        /// <summary>
        ///     Encrypt the given string using .  The string can be decrypted using
        ///     Decrypt().  The password parameters must match.
        /// </summary>
        public override string Encrypt(string textToBeEncrypted, string password)
        {
            ParameterCheck(textToBeEncrypted, password);

            string result; // Encrypted string to return
            RijndaelManaged aesObj = null; // RijndaelManaged object used to encrypt the data.

            try
            {
                // generate the key from the shared secret and the salt
                var key = new Rfc2898DeriveBytes(password, Salt);

                // Create a RijndaelManaged object
                aesObj = new RijndaelManaged();
                aesObj.Key = key.GetBytes(aesObj.KeySize/8);

                // Create a decryptor to perform the stream transform.
                var encryptor = aesObj.CreateEncryptor(aesObj.Key, aesObj.IV);

                // Create the streams used for encryption.
                using (var ms = new MemoryStream())
                {
                    // prepend the IV
                    ms.Write(BitConverter.GetBytes(aesObj.IV.Length), 0, sizeof (int));
                    ms.Write(aesObj.IV, 0, aesObj.IV.Length);
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (var sw = new StreamWriter(cs))
                        {
                            //Write all data to the stream.
                            sw.Write(textToBeEncrypted);
                        }
                    }
                    result = Convert.ToBase64String(ms.ToArray());
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesObj != null)
                    aesObj.Clear();
            }

            // Return the encrypted bytes from the memory stream.
            return result;
        }

        /// <summary>
        ///     Decrypt the given string.  Assumes the string was encrypted using
        ///     Encrypt(), using an identical password.
        /// </summary>
        public override string Decrypt(string textToBeDecrypted, string password)
        {
            ParameterCheck(textToBeDecrypted, password);

            // Declare the RijndaelManaged object
            // used to decrypt the data.
            RijndaelManaged aesObj = null;

            // Declare the string used to hold
            // the decrypted text.
            string result;

            try
            {
                // generate the key from the shared secret and the salt
                var key = new Rfc2898DeriveBytes(password, Salt);

                // Create the streams used for decryption.                
                var bytes = Convert.FromBase64String(textToBeDecrypted);
                using (var ms = new MemoryStream(bytes))
                {
                    // Create a RijndaelManaged object
                    // with the specified key and IV.
                    aesObj = new RijndaelManaged();
                    aesObj.Key = key.GetBytes(aesObj.KeySize/8);
                    // Get the initialization vector from the encrypted stream
                    aesObj.IV = ReadByteArray(ms);
                    // Create a decrytor to perform the stream transform.
                    var decryptor = aesObj.CreateDecryptor(aesObj.Key, aesObj.IV);
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (var sr = new StreamReader(cs))

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            result = sr.ReadToEnd();
                    }
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesObj != null)
                    aesObj.Clear();
            }

            return result;
        }
    }
}