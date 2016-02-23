using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace FunctionLib.Helper
{
    public static class EncryptionMethodHelper
    {
        public static IDictionary<string, Type> ImplementationList
        {
            get
            {
                var currentLoadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes());
                var implementations =
                    currentLoadedAssemblies.Where(
                        p =>
                            typeof (SymmetricAlgorithm).IsAssignableFrom(p) && p.BaseType == typeof (SymmetricAlgorithm));
                var dict = implementations.ToDictionary(implementation => implementation.Name, GetFirstImplementation);
                return dict;
            }
        }

        private static Type GetFirstImplementation(Type implementation)
        {
            var currentLoadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes());
            var result = currentLoadedAssemblies.FirstOrDefault(x => x.BaseType == implementation);
            return result ?? implementation;
        }
    }
}