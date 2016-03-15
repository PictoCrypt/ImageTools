// REFERENCE http://www.codeproject.com/Articles/483490/XCrypt-Encryption-and-decryption-class-wrapper

/* 
  Copright 2001-2003 by Markus Hahn <markus_hahn@gmx.net>
  All rights reserved. 
  See Documentation for license details.
*/


using System;
using System.Security.Cryptography;

namespace FunctionLib.Cryptography.Blowfish
{
    public class Blowfish : SymmetricAlgorithm, ICryptoTransform
    {
        private readonly bool m_blIsEncryptor;
        // in factory mode the BlowfishBase instances are always null,
        // they get only initialized in transformation mode

        private readonly BlowfishBase mBlowfishBase;
        private readonly BlowfishBaseCbc mBlowfishBaseCbc;


        // factory settings

        private RNGCryptoServiceProvider m_rng;

        /// <summary>
        ///     constructor
        /// </summary>
        public Blowfish()
        {
            mBlowfishBase = null;
            mBlowfishBaseCbc = null;

            // FIXME: are we supposed to create a default key and IV?
            IVValue = null;
            KeyValue = null;
            KeySizeValue = BlowfishBase.MAXKEYLENGTH*8;

            LegalBlockSizesValue = new KeySizes[1];
            LegalBlockSizesValue[0] = new KeySizes(BlockSize, BlockSize, 8);

            LegalKeySizesValue = new KeySizes[1];
            LegalKeySizesValue[0] = new KeySizes(0, BlowfishBase.MAXKEYLENGTH*8, 8);

            ModeValue = CipherMode.ECB;

            m_rng = null;
        }

        private Blowfish
            (
            byte[] key,
            byte[] iv,
            bool blCBC,
            bool blIsEncryptor
            )
        {
            if (null == key) GenerateKey();
            else Key = key;

            if (blCBC)
            {
                if (null == iv) GenerateIV();
                else IV = iv;

                mBlowfishBase = null;
                mBlowfishBaseCbc = new BlowfishBaseCbc(KeyValue, IVValue);
            }
            else
            {
                mBlowfishBase = new BlowfishBase(KeyValue);
                mBlowfishBaseCbc = null;
            }

            m_blIsEncryptor = blIsEncryptor;
        }


        // SymmetricAlgorithm ...


        /// <summary>
        ///     the BlowfishBase block size, can only be set to the same value
        /// </summary>
        public override int BlockSize
        {
            get { return BlowfishBase.BLOCKSIZE*8; }
            set
            {
                // (we only support 64bit block sizes, although BlowfishBase is
                //  also thinkable with 128bit blocks or even more)
                if (value != BlowfishBase.BLOCKSIZE*8)
                {
                    throw new CryptographicException("illegal blocksize");
                }
            }
        }

        /// <summary>
        ///     the initialization vector, used in the factory
        /// </summary>
        public override byte[] IV
        {
            get { return IVValue; }
            set
            {
                if (null == value)
                {
                    throw new ArgumentNullException();
                }
                if (value.Length != BlowfishBase.BLOCKSIZE)
                {
                    throw new CryptographicException("illegal IV length");
                }
                IVValue = value;
            }
        }

        /// <summary>
        ///     the key, used in the factory
        /// </summary>
        public override byte[] Key
        {
            get { return KeyValue; }
            set
            {
                if (null == value)
                {
                    throw new ArgumentNullException("key cannot be null");
                }
                // FIXME: according to the documentation we don't validate the
                //        key, can this be correct? additionally it is confusing
                //        because the key size can actually be changed, so what
                //        happens then to the key material already set?
                KeyValue = value;
            }
        }

        /// <summary>
        ///     the key length (for auto generation only)
        /// </summary>
        public override int KeySize
        {
            get { return KeySizeValue; }
            set
            {
                var ks = LegalKeySizes[0];
                if ((0 != value%ks.SkipSize) ||
                    (value > ks.MaxSize) ||
                    (value < ks.MinSize))
                {
                    throw new CryptographicException("invalid key size");
                }
                KeySizeValue = value;
            }
        }

        public override KeySizes[] LegalBlockSizes
        {
            get { return LegalBlockSizesValue; }
        }

        public override KeySizes[] LegalKeySizes
        {
            get { return LegalKeySizesValue; }
        }

        public override CipherMode Mode
        {
            get { return ModeValue; }
            set
            {
                // FIXME: we only support ECB and CBC, so is it ok
                //        to raise an exception even if the requested
                //        mode itself is a valid one?
                if (value != CipherMode.CBC &&
                    value != CipherMode.ECB)
                {
                    throw new CryptographicException("only ECB and CBC are supported");
                }
                ModeValue = value;
            }
        }

        // ICryptoTransform ...

        public bool CanReuseTransform
        {
            get { return true; }
        }

        public bool CanTransformMultipleBlocks
        {
            get { return true; }
        }

        public int InputBlockSize
        {
            get { return BlowfishBase.BLOCKSIZE; }
        }

        public int OutputBlockSize
        {
            get { return BlowfishBase.BLOCKSIZE; }
        }

        public int TransformBlock
            (
            byte[] bufIn,
            int nOfsIn,
            int nCount,
            byte[] bufOut,
            int nOfsOut
            )
        {
            // NOTE: we assume that the caller understands the meaning of
            //	     this method and that only even byte boundaries are
            //       given, thus we do not cache any data left internally

            var nResult = 0;

            if (null != mBlowfishBaseCbc)
            {
                if (m_blIsEncryptor)
                {
                    nResult = mBlowfishBaseCbc.Encrypt(bufIn, bufOut, nOfsIn, nOfsOut, nCount);
                }
                else
                {
                    nResult = mBlowfishBaseCbc.Decrypt(bufIn, bufOut, nOfsIn, nOfsOut, nCount);
                }
            }
            else if (null != mBlowfishBase)
            {
                if (m_blIsEncryptor)
                {
                    nResult = mBlowfishBase.Encrypt(bufIn, bufOut, nOfsIn, nOfsOut, nCount);
                }
                else
                {
                    nResult = mBlowfishBase.Decrypt(bufIn, bufOut, nOfsIn, nOfsOut, nCount);
                }
            }
            else
            {
                nResult = 0;
            }

            return nResult*BlowfishBase.BLOCKSIZE;
        }

        public byte[] TransformFinalBlock
            (
            byte[] inBuf,
            int nOfs,
            int nCount
            )
        {
            byte[] result;

            if (m_blIsEncryptor)
            {
                // we need to take over whatever we got, pad it with the right
                // scheme and then encrypt or decrypt it

                var nRest = nCount%BlowfishBase.BLOCKSIZE;

                // NOTE: the padding schemes may result into different data length,
                //       while zero padding just fills up the last block PKCS7 might
                //       need an extra block to store its length information

                var nBufSize = nCount - nRest;
                int nFill;

                if (PaddingMode.PKCS7 == PaddingValue)
                {
                    nBufSize += BlowfishBase.BLOCKSIZE;
                    nFill = BlowfishBase.BLOCKSIZE - nRest;
                }
                else
                {
                    if (0 < nRest) nBufSize += BlowfishBase.BLOCKSIZE;
                    nFill = 0;
                }

                result = new byte[nBufSize];
                Array.Copy(inBuf, nOfs, result, 0, nCount);

                for (var nI = nCount; nI < nBufSize; nI++)
                {
                    result[nI] = (byte) nFill;
                }

                TransformBlock(result, 0, nBufSize, result, 0);
            }
            else
            {
                var lastBlocks = new byte[nCount];
                if (0 < nCount)
                {
                    TransformBlock(inBuf, nOfs, nCount, lastBlocks, 0);

                    if (PaddingMode.PKCS7 == PaddingValue)
                    {
                        nCount -= lastBlocks[nCount - 1];
                    }

                    result = new byte[nCount];
                    Array.Copy(lastBlocks, 0, result, 0, nCount);
                }
                else
                {
                    // (that will be an empty array)
                    result = lastBlocks;
                }
            }

            return result;
        }


        /// <summary>
        ///     checks if a key is weak (and perhaps shouldn't be used)
        /// </summary>
        /// <param name="key">
        ///     the key material to test against
        /// </param>
        /// <returns>
        ///     true: key is "weak" / false: key passed the test
        /// </returns>
        public static bool IsWeakKey
            (byte[] key)
        {
            var bfAlg = new Blowfish(key, null, false, true);

            return bfAlg.mBlowfishBase.IsWeakKey;
        }

        // the factory methods are just simple mappings to the private constructors

        public override ICryptoTransform CreateEncryptor
            (
            byte[] key,
            byte[] iv
            )
        {
            var result = new Blowfish(
                key,
                iv,
                CipherMode.CBC == ModeValue,
                true);

            result.Padding = Padding;
            return result;
        }

        public override ICryptoTransform CreateDecryptor
            (
            byte[] key,
            byte[] iv
            )
        {
            var result = new Blowfish(
                key,
                iv,
                CipherMode.CBC == ModeValue,
                false);

            result.Padding = Padding;
            return result;
        }

        // FIXME: is our generator strategy here good enough?
        //        (and why does the base class actually not offer a decent default?)

        public override void GenerateKey()
        {
            if (null == m_rng) m_rng = new RNGCryptoServiceProvider();

            KeyValue = new byte[KeySizeValue/8];
            m_rng.GetBytes(KeyValue);
        }

        public override void GenerateIV()
        {
            if (null == m_rng) m_rng = new RNGCryptoServiceProvider();

            IVValue = new byte[BlowfishBase.BLOCKSIZE];
            m_rng.GetBytes(IVValue);
        }
    }
}