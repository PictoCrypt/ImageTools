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

        public static Bitmap Encrypt(object obj, Type method, Bitmap src, string value, string password, int additionalParam)
        {
            var baseType = typeof (SteganographicAlgorithmBase);
            var extractedMethod = baseType.GetMethods().FirstOrDefault(x => x.IsGenericMethod && x.Name == "Encrypt");
            if (extractedMethod != null)
            {
                return (Bitmap) extractedMethod.MakeGenericMethod(method)
                    .Invoke(obj, new object[] {src, value, password, additionalParam});
            }
            throw new ArgumentException(baseType.ToString());
        }

        public static object Decrypt(object obj, Type method, Bitmap src, string password, int additionalParam)
        {
            var baseType = typeof (SteganographicAlgorithmBase);
            var extractedMethod = baseType.GetMethods().FirstOrDefault(x => x.IsGenericMethod && x.Name == "Decrypt");
            if (extractedMethod != null)
            {
                return extractedMethod.MakeGenericMethod(method)
                    .Invoke(obj, new object[] {src, password, additionalParam});
            }
            throw new ArgumentException(baseType.ToString());
        }

        public static Bitmap Encrypt<T>(Bitmap src, string value, string password, int additionalParam)
            where T : SteganographicAlgorithm, new()
        {
            mLastAccessedAlgorithm = new T();
            var result = mLastAccessedAlgorithm.Encrypt(src, value, password, additionalParam);
            return result;
        }


        public static object Decrypt<T>(Bitmap src, string password, int additionalParam)
            where T : SteganographicAlgorithm, new()
        {
            mLastAccessedAlgorithm = new T();
            var result = mLastAccessedAlgorithm.Decrypt(src, password, additionalParam);
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
    }
}