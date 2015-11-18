using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace FunctionLib.Helper
{
    public class EncryptionMethodHelper
    {
        private static EncryptionMethodHelper mInstance;

        public List<Type> ImplementationList
        {
            get
            {
                var implementations = new HashSet<Type>(
                    AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(s => s.GetTypes())
                        .Where(p => typeof (SymmetricAlgorithm).IsAssignableFrom(p) & p.BaseType == typeof(SymmetricAlgorithm)));
                return implementations.ToList();
            }
        }

        public static EncryptionMethodHelper Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new EncryptionMethodHelper();
                }
                return mInstance;
            }
        }
    }
}