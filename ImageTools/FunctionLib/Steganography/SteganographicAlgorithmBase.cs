using System;
using System.Drawing;
using System.Linq;

namespace FunctionLib.Steganography
{
    public class SteganographicAlgorithmBase
    {
        private static SteganographicAlgorithm mLastAccessedAlgorithm;

        public static Bitmap Encrypt(object obj, SteganographicMethod method, Bitmap src, string value)
        {
            var encryptionType = MethodNameToType(method);
            var baseType = typeof (SteganographicAlgorithmBase);
            var extractedMethod = baseType.GetMethods().FirstOrDefault(x => x.IsGenericMethod && x.Name == "Encrypt");
            if (extractedMethod != null)
            {
                return (Bitmap) extractedMethod.MakeGenericMethod(encryptionType)
                    .Invoke(obj, new object[] {src, value});
            }
            throw new ArgumentException(baseType.ToString());
        }

        public static string Decrypt(object obj, SteganographicMethod method, Bitmap src)
        {
            var encryptionType = MethodNameToType(method);
            var baseType = typeof (SteganographicAlgorithmBase);
            var extractedMethod = baseType.GetMethods().FirstOrDefault(x => x.IsGenericMethod && x.Name == "Decrypt");
            if (extractedMethod != null)
            {
                return extractedMethod.MakeGenericMethod(encryptionType)
                    .Invoke(obj, new object[] {src}).ToString();
            }
            throw new ArgumentException(baseType.ToString());
        }

        public static Bitmap Encrypt<T>(Bitmap src, string value)
            where T : SteganographicAlgorithm, new()
        {
            mLastAccessedAlgorithm = new T();
            var result = mLastAccessedAlgorithm.Encrypt(src, value);
            return result;
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

        private static Type MethodNameToType(SteganographicMethod method)
        {
            switch (method)
            {
                case SteganographicMethod.LSB:
                    return typeof (LeastSignificantBit);
            }
            throw new ArgumentOutOfRangeException(nameof(method), method, null);
        }
    }
}