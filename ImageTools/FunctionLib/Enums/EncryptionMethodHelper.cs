using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace FunctionLib.Enums
{
    public class EncryptionMethodHelper
    {
        private static EncryptionMethodHelper mInstance;

        public List<Type> ImplementationList
        {
            get
            {
                var allClasses = new HashSet<Type>(
                    AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(s => s.GetTypes())
                        .Where(p => typeof (SymmetricAlgorithm).IsAssignableFrom(p)));
                var firstImplementations = allClasses.Where(t => t.BaseType == typeof(SymmetricAlgorithm));
                return firstImplementations.ToList();
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