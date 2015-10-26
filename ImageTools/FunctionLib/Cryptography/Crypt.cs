using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FunctionLib.Cryptography
{
    public abstract class Crypt : SymmetricAlgorithm
    {
        protected static readonly byte[] Salt = Encoding.ASCII.GetBytes("jasdh7834y8hfeur73rsharks214");

        ///// <summary>
        ///// Encrypts a given string with RSA (RFC 2898) with an iteration count of 5000.
        ///// </summary>
        ///// <param name="password">The given password, which should be encrypted.</param>
        ///// <param name="rfcIterationCount">The range of the encryption Method. Standard is 5000.</param>
        ///// <param name="keyLength">The length of the Password after encryption. Standard is 32.</param>
        ///// <returns></returns>
        //private string EncryptPassword(string password, int rfcIterationCount = 5000, int keyLength = 32) 
        //{
        //    // Was hier steht hat einen Einfluss darauf, wie das Kennwort später aussehen wird. Die Werte müssen jedoch keiner bestimmten Struktur folgen und sollten möglichst zufällig sein.
        //    var salt = new byte[] { 0x1, 0x2, 0x3, 0x4, 0x5 };
        //    // Welche Länge soll das Passwort später besitzen ? Oft sind es 32 Zeichen => 256 Bits.
        //    //const int keyLength = 32; 

        //    // Schlüssel generieren
        //    var rfc2898 = new Rfc2898DeriveBytes(password, salt)
        //    // Der Standard sind 1000, aber mehr schadet nicht.
        //    {
        //        IterationCount = rfcIterationCount
        //    };
        //    var resultingKey = rfc2898.GetBytes(keyLength);

        //    // Verschlüsseln
        //    var rawData = "Dies hier möchten wir verschlüsseln.";
        //    byte[] data = Encoding.ASCII.GetBytes(rawData); // Dies wird aus dem Text ein Byte-Array machen. Es gibt verschiedene Encoder, aber ich bevorzuge ASCII, da es schnell ist und Arrays geringer Größe erzeugt.
        //    byte[] chiffre = UltimateEncryption(data, resultingKey); // Frei erfunden. Hier musst du einen Algorithmus wählen.

        //    // Abspeichern
        //    File.WriteAllBytes(@"X:\Ordner\Datei.ext", chiffre);

        //    // Roundtrip => Wir prüfen, ob wir wieder den ursprünglichen Text erhalten. Das ist nicht nötig, aber zum Üben anschaulich.
        //    byte[] roundtrip = UltimateDecryption(data, resultingKey); // Der entsprechende Algorithmus, um den Text wieder zu entschlüsseln.
        //    string decrypted = Encoding.ASCII.GetString(roundtrip); // Der gleiche Encoder wie vorher auch. Hier ASCII.

        //    // Überprüfen => Hat es geklappt ?
        //    if (decrypted == rawdata)
        //        JumpInTheAirAndShoutHurray("!");
        //}

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

        public Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public byte[] StringToByteArray(string str)
        {
            var result = new ASCIIEncoding();
            return result.GetBytes(str);
        }

        public string ByteArrayToString(byte[] arr)
        {
            var result = new ASCIIEncoding();
            return result.GetString(arr);
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