using System;
using System.Collections.Generic;
using System.Linq;
using FunctionLib.Helper;

namespace FunctionLib.Steganography
{
    public class RandomLsb : LsbAlgorithmBase
    {
        protected override LockBitmap Encrypt(LockBitmap src, byte[] value, int password = 0,
            int significantIndicator = 3)
        {
            var random = new Random(password);
            if (value == null)
            {
                throw new ArgumentException("'value' is null.");
            }

            var byteIndex = 0;
            var bitIndex = 0;
            var bytes = value.ToList();
            while (true)
            {
                var x = GetNextRandom("x", src.Width, random);
                var y = GetNextRandom("y", src.Height, random);

                Embed(src, x, y, significantIndicator, ref bytes, ref byteIndex, ref bitIndex);
                if (byteIndex > bytes.Count - 1 || byteIndex == bytes.Count - 1 && bitIndex == 7)
                {
                    return src;
                }
            }
        }

        protected override byte[] Decrypt(LockBitmap src, int password = 0, int significantIndicator = 3)
        {
            var random = new Random(password);
            var byteList = new List<byte>();
            var bitHolder = new List<int>();
            while (true)
            {
                var x = GetNextRandom("x", src.Width, random);
                var y = GetNextRandom("y", src.Height, random);

                ReadEmbedded(src, x, y, significantIndicator, ref byteList);
                // Check for EndTag (END)
                var index = MethodHelper.IndexOfWithinLastTwo(byteList);
                if (index > -1)
                {
                    // Remove overhang bytes
                    if (byteList.Count > index + Constants.EndTag.Length)
                    {
                        byteList.RemoveRange(index + Constants.EndTag.Length,
                            byteList.Count - (index + Constants.EndTag.Length));
                    }
                    return byteList.ToArray();
                }
            }
        }
    }
}