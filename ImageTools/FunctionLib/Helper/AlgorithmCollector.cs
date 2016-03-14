using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FunctionLib.Helper
{
    public static class AlgorithmCollector
    {
        public static List<T> GetAllAlgorithm<T>()
        {
            var objects = Assembly.GetAssembly(typeof(T))
                .GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract
                    && myType.IsSubclassOf(typeof(T)))
                .Select(type => (T)Activator.CreateInstance(type))
                .ToList();
            return objects;
        }
    }
}