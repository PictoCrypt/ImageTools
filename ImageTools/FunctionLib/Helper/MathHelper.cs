using System.Linq;
using static System.Int32;

namespace FunctionLib.Helper
{
    public static class MathHelper
    {
        public static int Max(int[] args)
        {
            var max = args.Concat(new[] {MinValue}).Max();
            return max;
        }
    }
}