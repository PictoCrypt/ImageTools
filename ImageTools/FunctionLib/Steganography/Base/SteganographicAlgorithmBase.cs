using System;
using System.Drawing;
using System.Linq;
using FunctionLib.Model;

namespace FunctionLib.Steganography.Base
{
    public class SteganographicAlgorithmBase
    {
        private static ISteganographicAlgorithm mLastAccessedAlgorithm;

        public static int ChangedPixels
        {
            get { return mLastAccessedAlgorithm.ChangedPixels.Count; }
        }

        public static Bitmap Encrypt(object obj, Type method, Bitmap src, string value, int password,
            int additionalParam)
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

        public static object Decrypt(object obj, Type method, Bitmap src, int password, int additionalParam)
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

        public static Bitmap Encrypt<T>(Bitmap src, MessageImpl value, int password, int additionalParam)
            where T : ISteganographicAlgorithm, new()
        {
            mLastAccessedAlgorithm = new T();
            var result = mLastAccessedAlgorithm.Encode(src, value, password, additionalParam);
            result.UnlockBits();
            return result.Source;
        }


        public static object Decrypt<T>(Bitmap src, int password, int additionalParam)
            where T : ISteganographicAlgorithm, new()
        {
            mLastAccessedAlgorithm = new T();
            var result = mLastAccessedAlgorithm.Decode(src, password, additionalParam);
            return result;
        }

        //TODO: Generic
        public static string ChangeColor(string srcPath, Color color)
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