using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FunctionLib.Helper;
using FunctionLib.Steganography.Base;

namespace FunctionLib.Steganography.LSB
{
    public abstract class LsbAlgorithmBase : SteganographicAlgorithmImpl
    {
        protected List<byte> DecryptHelper(List<byte> bytes, ref ICollection<int> bitHolder)
        {
            while (bitHolder.Count >= 8)
            {
                var sequence = bitHolder.Take(8);
                var builder = new StringBuilder();
                foreach (var x in sequence)
                {
                    builder.Append(x.ToString());
                }
                bytes.Add(Convert.ToByte(builder.ToString(), 2));
                bitHolder = bitHolder.Skip(8).ToList();
            }
            return bytes;
        } 

        protected byte CurrentByte(IReadOnlyList<byte> b, ref int byteIndex, ref int bitIndex, int significantIndicator)
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
    }
}