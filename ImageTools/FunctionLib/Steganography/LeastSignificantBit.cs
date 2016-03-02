using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FunctionLib.Helper;

namespace FunctionLib.Steganography
{
    public class LeastSignificantBit : LsbAlgorithmBase
    {
        protected override LockBitmap Encrypt(LockBitmap src, byte[] value, int password = 0,
            int significantIndicator = 3)
        {
            if (value == null)
            {
                throw new ArgumentException("'value' is null.");
            }

            var byteIndex = 0;
            var bitIndex = 0;
            var bytes = value.ToList();

            for (var y = 0; y < src.Height; y++)
            {
                for (var x = 0; x < src.Width; x++)
                {
                    Embed(src, x, y, significantIndicator, ref bytes, ref byteIndex, ref bitIndex);
                    if (byteIndex > bytes.Count - 1 || byteIndex == bytes.Count - 1 && bitIndex == 7)
                    {
                        return src;
                    }
                }
            }
            return src;
        }

        private byte CurrentByte(List<byte> b, ref int byteIndex, ref int bitIndex, int significantIndicator)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < significantIndicator; i++)
            {
                if (bitIndex == 8)
                {
                    byteIndex++;
                    bitIndex = 0;
                }
                var bit = byteIndex >= b.Count ? 0 : ByteHelper.GetBit(b[byteIndex], bitIndex++);
                builder.Append(bit);
            }

            var result = Convert.ToByte(builder.ToString(), 2);
            return result;
        }

        protected override byte[] Decrypt(LockBitmap src, int password = 0, int significantIndicator = 3)
        {
            var byteList = new List<byte>();
            for (var y = 0; y < src.Height; y++)
            {
                for (var x = 0; x < src.Width; x++)
                {
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
            throw new SystemException("Error, anything happened (or maybe not).");
        }
    }
}