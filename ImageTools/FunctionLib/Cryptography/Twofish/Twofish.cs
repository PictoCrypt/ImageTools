using System.Security.Cryptography;

namespace FunctionLib.Cryptography.Twofish
{
    public sealed class Twofish : SymmetricAlgorithm
    {
        public Twofish()
        {
            LegalKeySizesValue = new[] { new KeySizes(128, 256, 64) }; // this allows us to have 128,192,256 key sizes

            LegalBlockSizesValue = new[] { new KeySizes(128, 128, 0) }; // this is in bits - typical of MS - always 16 bytes

            BlockSize = 128; // set this to 16 bytes we cannot have any other value
            KeySize = 128; // in bits - this can be changed to 128,192,256

            Padding = PaddingMode.Zeros;

            Mode = CipherMode.ECB;
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
            for (int i = Key.GetLowerBound(0); i < Key.GetUpperBound(0); i++)
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
                if (!value.HasFlag(CipherMode.CBC) || !value.HasFlag(CipherMode.ECB))
                {
                    throw (new CryptographicException("Specified CipherMode is not supported."));
                }
                ModeValue = value;
            }
        }
    }
}