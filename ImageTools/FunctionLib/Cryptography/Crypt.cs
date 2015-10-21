using System;
using System.Drawing;
using System.IO;
using System.Text;

namespace FunctionLib.Cryptography
{
    public abstract class Crypt
    {
        protected static readonly byte[] Salt = Encoding.ASCII.GetBytes("jasdh7834y8hfeur73rsharks214");

        /// <param name="textToBeEncrypted">The text to encrypt.</param>
        /// <param name="password">A password used to generate a key for encryption.</param>
        public abstract string Encrypt(string textToBeEncrypted, string password);

        /// <param name="textToBeDecrypted">The text to decrypt.</param>
        /// <param name="password">A password used to generate a key for decryption.</param>
        public abstract string Decrypt(string textToBeDecrypted, string password);

        protected static byte[] ReadByteArray(Stream s)
        {
            var rawLength = new byte[sizeof (int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
            {
                throw new SystemException("Stream did not contain properly formatted byte array");
            }

            var buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new SystemException("Did not read byte array properly");
            }

            return buffer;
        }

        protected static void ParameterCheck(string textToBeEncrypted, string password)
        {
            if (string.IsNullOrEmpty(textToBeEncrypted))
                throw new ArgumentNullException("textToBeEncrypted");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");
        }
    }
}