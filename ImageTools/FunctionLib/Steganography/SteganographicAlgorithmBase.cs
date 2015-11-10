using System;
using System.Drawing;
using System.Linq;

namespace FunctionLib.Steganography
{
    public class SteganographicAlgorithmBase
    {
        private static SteganographicAlgorithm mLastAccessedAlgorithm;

        public static int ChangedPixels
        {
            get { return mLastAccessedAlgorithm.ChangedPixels.Count; }
        }

        public static Bitmap Encrypt(object obj, SteganographicMethod method, Bitmap src, object value,
            int additionalParam)
        {
            var encryptionType = MethodNameToType(method);
            var baseType = typeof (SteganographicAlgorithmBase);
            var extractedMethod = baseType.GetMethods().FirstOrDefault(x => x.IsGenericMethod && x.Name == "Encrypt");
            if (extractedMethod != null)
            {
                return (Bitmap) extractedMethod.MakeGenericMethod(encryptionType)
                    .Invoke(obj, new[] {src, value, additionalParam});
            }
            throw new ArgumentException(baseType.ToString());
        }

        public static object Decrypt(object obj, SteganographicMethod method, Bitmap src, ResultingType type, int additionalParam)
        {
            var encryptionType = MethodNameToType(method);
            var baseType = typeof (SteganographicAlgorithmBase);
            var extractedMethod = baseType.GetMethods().FirstOrDefault(x => x.IsGenericMethod && x.Name == "Decrypt");
            if (extractedMethod != null)
            {
                return extractedMethod.MakeGenericMethod(encryptionType)
                    .Invoke(obj, new object[] {src, type, additionalParam});
            }
            throw new ArgumentException(baseType.ToString());
        }

        public static Bitmap Encrypt<T>(Bitmap src, object value, int additionalParam)
            where T : SteganographicAlgorithm, new()
        {
            mLastAccessedAlgorithm = new T();
            var result = mLastAccessedAlgorithm.Encrypt(src, value, additionalParam);
            return result;
        }


        public static object Decrypt<T>(Bitmap src, ResultingType type, int additionalParam)
            where T : SteganographicAlgorithm, new()
        {
            mLastAccessedAlgorithm = new T();
            var result = mLastAccessedAlgorithm.Decrypt(src, type, additionalParam);
            return result;
        }

        public static string ChangeColor(string srcPath, Color color)
        {
            return ChangeColor<ComplexLeastSignificantBit>(srcPath, color);
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
                case SteganographicMethod.ComplexLSB:
                    return typeof (ComplexLeastSignificantBit);
            }
            throw new ArgumentOutOfRangeException(nameof(method), method, null);
        }
    }
}