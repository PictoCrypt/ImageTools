using System;
using System.Text;

namespace FunctionLib.Helper
{
    public static class ByteHelper
    {
        /// <summary>
        ///     Gets the bit on the specific position in byte.
        /// </summary>
        /// <param name="b">Byte</param>
        /// <param name="index">Position index. Position of 0 is the most significant bit.</param>
        /// <returns>Bit on the specific position.</returns>
        public static int GetBit(byte b, int index)
        {
            var builder = new StringBuilder("00000000");
            builder.Remove(index, 1);
            builder.Insert(index, 1);

            var result = Convert.ToInt32(builder.ToString(), 2);
            return (b & result) > 0 ? 1 : 0;
        }

        /// <summary>
        ///     Clears the last significant bit according to the parameter of lsbIndicator.
        /// </summary>
        /// <param name="b">Byte</param>
        /// <param name="lsbIndicator">Least-Significant-Bits</param>
        /// <returns></returns>
        public static int ClearLeastSignificantBit(byte b, int lsbIndicator)
        {
            return b & GetByteMask(lsbIndicator, 8);
        }

        private static int GetByteMask(int startRange, int endRange)
        {
            int abyte = 0, abyte2 = 0;
            for (var i = 0; i < 8; i++)
            {
                byte bit;
                if (i <= endRange && i >= startRange)
                    bit = 1;
                else
                    bit = 0;
                abyte = (byte) (abyte << 1 | bit);
            }
            for (var i = 0; i < 8; i++)
            {
                abyte2 = abyte2 << 1 | ((abyte >> i) & 0x1);
            }
            return abyte2;
        }
    }
}