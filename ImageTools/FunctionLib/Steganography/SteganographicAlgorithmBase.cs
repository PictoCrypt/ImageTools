using System;
using System.Drawing;

namespace FunctionLib.Steganography
{
    public class SteganographicAlgorithmBase
    {
        private static SteganographicAlgorithm mLastAccessedAlgorithm;

        public static Bitmap Encrypt(Bitmap src, string value)
        {
            return Encrypt<LeastSignificantBit>(src, value);
        }

        public static Bitmap Encrypt<T>(Bitmap src, string value) 
            where T : SteganographicAlgorithm, new()
        {
            mLastAccessedAlgorithm = new T();
            var result = mLastAccessedAlgorithm.Encrypt(src, value);
            return result;
        }

        public static string Decrypt(Bitmap src)
        {
            return Decrypt<LeastSignificantBit>(src);
        }

        public static string Decrypt<T>(Bitmap src)
            where T : SteganographicAlgorithm, new()
        {
            mLastAccessedAlgorithm = new T();
            var result = mLastAccessedAlgorithm.Decrypt(src);
            return result;
        }

        public static string ChangeColor(string srcPath, Color color)
        {
            return ChangeColor<LeastSignificantBit>(srcPath, color);
        }

        public static string ChangeColor<T>(string srcPath, Color color)
            where T : SteganographicAlgorithm, new()
        {
            var result = "";
            if (mLastAccessedAlgorithm != null)
            {
                result = mLastAccessedAlgorithm.ChangeColor(srcPath, color);
            }
            return result;
        }
    }
}