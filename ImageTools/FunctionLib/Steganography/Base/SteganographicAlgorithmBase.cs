using System;
using System.Drawing;
using System.Linq;
using FunctionLib.Model;
using FunctionLib.Model.Message;

namespace FunctionLib.Steganography.Base
{
    public class SteganographicAlgorithmBase
    {
        private static ISteganographicAlgorithm mLastAccessedAlgorithm;

        public static int ChangedPixels
        {
            get { return mLastAccessedAlgorithm.ChangedPixels.Count; }
        }

        public static Bitmap Encode(object obj, Type method, Bitmap src, ISecretMessage value, int password,
            int additionalParam)
        {
            var baseType = typeof (SteganographicAlgorithmBase);
            var extractedMethod = baseType.GetMethods().FirstOrDefault(x => x.IsGenericMethod && x.Name == "Encode");
            if (extractedMethod != null)
            {
                return (Bitmap) extractedMethod.MakeGenericMethod(method)
                    .Invoke(obj, new object[] {src, value, password, additionalParam});
            }
            throw new ArgumentException(baseType.ToString());
        }

        public static ISecretMessage Decode(object obj, Type method, Bitmap src, int password, MessageType type, int additionalParam)
        {
            var baseType = typeof (SteganographicAlgorithmBase);
            var extractedMethod = baseType.GetMethods().FirstOrDefault(x => x.IsGenericMethod && x.Name == "Decode");
            if (extractedMethod != null)
            {
                return extractedMethod.MakeGenericMethod(method)
                    .Invoke(obj, new object[] {src, password, type, additionalParam}) as ISecretMessage;
            }
            throw new ArgumentException(baseType.ToString());
        }

        public static Bitmap Encode<T>(Bitmap src, ISecretMessage value, int password, int additionalParam)
            where T : ISteganographicAlgorithm, new()
        {
            mLastAccessedAlgorithm = new T();
            var result = mLastAccessedAlgorithm.Encode(src, value, password, additionalParam);
            result.UnlockBits();
            return result.Source;
        }


        public static ISecretMessage Decode<T>(Bitmap src, int password, MessageType type, int additionalParam)
            where T : ISteganographicAlgorithm, new()
        {
            mLastAccessedAlgorithm = new T();
            var result = mLastAccessedAlgorithm.Decode(src, password, type, additionalParam);
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