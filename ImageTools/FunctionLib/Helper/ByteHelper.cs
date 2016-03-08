using System;
using System.Text;

namespace FunctionLib.Helper
{
    public static class ByteHelper
    {
        /// <summary>
        /// Gets the bit on the specific position in byte.
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
        /// Clears the last significant bit according to the parameter of lsbIndicator.
        /// </summary>
        /// <param name="b">Byte</param>
        /// <param name="lsbIndicator">Least-Significant-Bits</param>
        /// <returns></returns>
        public static int ClearLeastSignificantBit(byte b, int lsbIndicator)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < 8 - lsbIndicator; i++)
            {
                builder.Append("1");
            }
            for (var i = 0; i < lsbIndicator; i++)
            {
                builder.Append("0");
            }

            var result = Convert.ToInt32(builder.ToString(), 2);
            return b & result;
        }
    }
}