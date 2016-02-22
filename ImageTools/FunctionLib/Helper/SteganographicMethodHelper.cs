using System;
using System.Collections.Generic;
using System.Linq;
using FunctionLib.Steganography;

namespace FunctionLib.Helper
{
    public static class SteganographicMethodHelper
    {
        public static List<Type> ImplementationList
        {
            get
            {
                var implementations = new HashSet<Type>(
                                AppDomain.CurrentDomain.GetAssemblies()
                                    .SelectMany(s => s.GetTypes())
                                    .Where(p => typeof(SteganographicAlgorithm).IsAssignableFrom(p) && p.BaseType == typeof(SteganographicAlgorithm)));
                return implementations.ToList();
            }
        }
    }
}