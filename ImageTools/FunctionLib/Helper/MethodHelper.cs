using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using FunctionLib.Model;
using FunctionLib.Model.Message;

namespace FunctionLib.Helper
{
    public static class MethodHelper
    {
        //private static bool CheckIfPossibleImage(int length)
        //{
        //    var result = length/3.0;
        //    var diff = Math.Abs(Math.Truncate(result) - result);
        //    return (diff < 0.0000001) || (diff > 0.9999999);
        //}

        public static int IndexOfWithinLastTwo(List<byte> byteList)
        {
            if (byteList.Count <= Constants.EndTag.Length)
            {
                return -1;
            }

            var seq1 = byteList.GetRange(byteList.Count - Constants.EndTag.Length, Constants.EndTag.Length);
            var seq2 = byteList.GetRange(byteList.Count - Constants.EndTag.Length - 1, Constants.EndTag.Length);

            if (seq1.SequenceEqual(Constants.EndTag))
            {
                return byteList.Count - Constants.EndTag.Length;
            }
            if (seq2.SequenceEqual(Constants.EndTag))
            {
                return byteList.Count - Constants.EndTag.Length - 1;
            }
            return -1;
        }

        public static ISecretMessage GetSpecificMessage(MessageType type, byte[] bytes)
        {
            switch (type)
            {
                case MessageType.Text:
                    return new TextMessage(bytes);
                case MessageType.Document:
                    return new DocumentMessage(bytes);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}