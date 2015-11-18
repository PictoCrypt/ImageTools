using System;
using System.Collections.Generic;
using System.Linq;
using FunctionLib.Steganography;

namespace FunctionLib.Helper
{
    public class SteganographicMethodHelper
    {
        private static SteganographicMethodHelper mInstance;

        public List<Type> ImplementationList
        {
            get
            {
                var implementations = new HashSet<Type>(
                                AppDomain.CurrentDomain.GetAssemblies()
                                    .SelectMany(s => s.GetTypes())
                                    .Where(p => typeof(SteganographicAlgorithm).IsAssignableFrom(p) & p.BaseType == typeof(SteganographicAlgorithm)));
                return implementations.ToList();
            }
        }

        public static SteganographicMethodHelper Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new SteganographicMethodHelper();
                }
                return mInstance;
            }
        }
    }
}