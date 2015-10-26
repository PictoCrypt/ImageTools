using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FunctionLib.Cryptography.Twofish
{
    public sealed class Twofish : Crypt
    {
        public Twofish(CipherMode mode = CipherMode.ECB)
        {
            LegalKeySizesValue = new[] { new KeySizes(128, 256, 64) }; // this allows us to have 128,192,256 key sizes

            LegalBlockSizesValue = new[] { new KeySizes(128, 128, 0) }; // this is in bits - typical of MS - always 16 bytes

            BlockSize = 128; // set this to 16 bytes we cannot have any other value
            KeySize = 128; // in bits - this can be changed to 128,192,256

            Padding = PaddingMode.Zeros;

            Mode = mode;
        }

        /// <summary>
        /// Creates an object that supports ICryptoTransform that can be used to encrypt data using the Twofish encryption algorithm.
        /// </summary>
        /// <param name="key">A byte array that contains a key. The length of this key should be equal to the KeySize property</param>
        /// <param name="iv">A byte array that contains an initialization vector. The length of this IV should be equal to the BlockSize property</param>
        public override ICryptoTransform CreateEncryptor(byte[] key, byte[] iv)
        {
            Key = key; // this appears to make a new copy

            if (Mode == CipherMode.CBC)
                IV = iv;

            return new TwofishEncryption(KeySize, ref KeyValue, ref IVValue, ModeValue, TwofishBase.EncryptionDirection.Encrypting);
        }

        /// <summary>
        /// Creates an object that supports ICryptoTransform that can be used to decrypt data using the Twofish encryption algorithm.
        /// </summary>
        /// <param name="key">A byte array that contains a key. The length of this key should be equal to the KeySize property</param>
        /// <param name="iv">A byte array that contains an initialization vector. The length of this IV should be equal to the BlockSize property</param>
        public override ICryptoTransform CreateDecryptor(byte[] key, byte[] iv)
        {
            Key = key;

            if (Mode == CipherMode.CBC)
                IV = iv;

            return new TwofishEncryption(KeySize, ref KeyValue, ref IVValue, ModeValue, TwofishBase.EncryptionDirection.Decrypting);
        }

        /// <summary>
        /// Generates a random initialization Vector (IV). 
        /// </summary>
        public override void GenerateIV()
        {
            IV = new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        }

        /// <summary>
        /// Generates a random Key. This is only really useful in testing scenarios.
        /// </summary>
        public override void GenerateKey()
        {
            Key = new byte[KeySize / 8];

            // set the array to all 0 - implement a random key generation mechanism later probably based on PRNG
            for (var i = Key.GetLowerBound(0); i < Key.GetUpperBound(0); i++)
            {
                Key[i] = 0;
            }
        }

        /// <summary>
        /// Override the Set method on this property so that we only support CBC and EBC
        /// </summary>
        public override CipherMode Mode
        {
            set
            {
                switch (value)
                {
                    case CipherMode.CBC:
                        break;
                    case CipherMode.ECB:
                        break;
                    default:
                        throw (new CryptographicException("Specified CipherMode is not supported."));
                }
                this.ModeValue = value;
            }
        }

        public override string Encrypt(string textToBeEncrypted, string password)
        {
            var ms = new MemoryStream();

            // create an encoder
            ICryptoTransform encode = new ToBase64Transform();

            //create Twofish Encryptor from this instance
            var plainText = StringToByteArray(textToBeEncrypted);
            // we use the plainText as the IV as in ECB mode the IV is not used
            var encrypt = CreateEncryptor(Key, plainText); 

            // we have to work backwords defining the last link in the chain first
            var cryptostreamEncode = new CryptoStream(ms, encode, CryptoStreamMode.Write);
            var cryptostream = new CryptoStream(cryptostreamEncode, encrypt, CryptoStreamMode.Write);

            cryptostream.Write(plainText, 0, plainText.Length);
            cryptostream.Close();
            return Encoding.Default.GetString(ms.ToArray());
        }

        public override string Decrypt(string textToBeDecrypted, string password)
        {
            // create a decoder
            ICryptoTransform decode = new FromBase64Transform();

            var plainText = StringToByteArray(textToBeDecrypted);
            //create DES Decryptor from our des instance
            var decrypt = CreateDecryptor(Key, plainText);

            var msD = new MemoryStream();

            //create crypto stream set to read and do a Twofish decryption transform on incoming bytes
            var cryptostreamD = new CryptoStream(msD, decrypt, CryptoStreamMode.Write);
            var cryptostreamDecode = new CryptoStream(cryptostreamD, decode, CryptoStreamMode.Write);
            
            //write out the decrypted stream
            cryptostreamDecode.Write(plainText, 0, plainText.Length);

            cryptostreamDecode.Close();

            return Encoding.Default.GetString(msD.ToArray());
        }
    }
}