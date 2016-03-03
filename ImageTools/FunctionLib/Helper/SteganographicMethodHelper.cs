using System;
using System.Collections.Generic;
using System.Linq;
using FunctionLib.Steganography.Base;

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
                        .Where(
                            p =>
                                typeof (ISteganographicAlgorithm).IsAssignableFrom(p) &&
                                p.BaseType == typeof (ISteganographicAlgorithm)));
                return implementations.ToList();
            }
        }
    }
}