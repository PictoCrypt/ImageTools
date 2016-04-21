using System;
using System.Collections.Generic;
using System.Linq;
using FunctionLib.Cryptography;
using FunctionLib.Helper;
using FunctionLib.Steganography;

namespace ImageToolCommandPrompt
{
    public abstract class CommandTool
    {
        public static readonly string Seperator = "--------------------";
        private readonly bool mParametersSet;

        protected CommandTool()
        {
        }

        protected CommandTool(string[] args)
        {
            mParametersSet = false;
            for (var i = 0; i < args.Count(x => x.StartsWith("-")); i++)
            {
                mParametersSet = true;
                MapArgument(args[i++], args[i]);
            }
        }

        protected abstract string Name { get; }

        protected abstract bool Configured { get; }

        protected abstract void MapArgument(string map, string parameter);

        public void Run()
        {
            if (mParametersSet)
            {
                RunWithParameters();
            }
            else
            {
                RunSetup();
            }
        }

        protected abstract void RunSetup();

        protected abstract void RunWithParameters();

        public abstract void Help();

        protected void InitializeHelp()
        {
            Console.WriteLine();
            Console.WriteLine(Seperator + " {0} " + Seperator, Name);
            Console.WriteLine("{0} \t Starts the benchmarking setup without parameters.", Name);
            Console.WriteLine("{0} [", Name);
        }

        protected void InitializeSetup()
        {
            Console.WriteLine(Seperator + " {0}-SETUP " + Seperator, Name);
        }

        protected static void WriteError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();

            //TODO better error handling?
            Environment.Exit(0xA0);
        }

        protected CryptographicAlgorithmImpl GetCrypt(int index)
        {
            var algorithms = AlgorithmCollector.GetAllAlgorithm<CryptographicAlgorithmImpl>();
            return algorithms[index];
        }

        protected SteganographicAlgorithmImpl GetStego(int index)
        {
            var algorithms = AlgorithmCollector.GetAllAlgorithm<SteganographicAlgorithmImpl>();
            return algorithms[index];
        }

        //protected SteganographicAlgorithmImpl SteganographicAlgorithmChooser()
        //{
        //    Console.WriteLine(Seperator + " Steganographic Algorithm Chooser " + Seperator);
        //    var algorithms = AlgorithmCollector.GetAllAlgorithm<SteganographicAlgorithmImpl>();
        //    return algorithms[AlgorithmChooser(algorithms.Select(x => x.Name))];
        //}

        //protected CryptographicAlgorithmImpl CryptographicAlgorithmChooser()
        //{
        //    Console.WriteLine(Seperator + " Cryptographic Algorithm Chooser " + Seperator);
        //    var algorithms = AlgorithmCollector.GetAllAlgorithm<CryptographicAlgorithmImpl>();
        //    return algorithms[AlgorithmChooser(algorithms.Select(x => x.Name))];
        //}

        private List<T> GetAllAlgorithms<T>()
        {
            return AlgorithmCollector.GetAllAlgorithm<T>();
        }

        protected int AlgorithmChooser<T>()
        {
            Console.WriteLine("{0} \t {1}", "Index", "Algorithm Name");
            var index = 0;
            foreach (var algorithm in GetAllAlgorithms<T>())
            {
                Console.WriteLine("{0} \t {1}", index++, algorithm);
            }
            Console.WriteLine("Please choose an algorithm by typing the index:");
            index = int.Parse(Console.ReadLine());
            return index;
        }

        protected int AlgorithmChooser(IEnumerable<string> algorithms)
        {
            Console.WriteLine("{0} \t {1}", "Index", "Algorithm Name");
            var index = 0;
            foreach (var algorithm in algorithms)
            {
                Console.WriteLine("{0} \t {1}", index++, algorithm);
            }
            Console.WriteLine("Please choose an algorithm by typing the index:");
            index = int.Parse(Console.ReadLine());
            return index;
        }

        protected bool GetBool(string readLine)
        {
            if (!string.IsNullOrEmpty(readLine))
            {
                if (readLine.Equals("y", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        protected void InitializeRun()
        {
            Console.WriteLine("Press <ENTER> to start {0}.", Name);
            Console.ReadLine();
            Console.WriteLine(Seperator + " {0} " + Seperator, Name);
        }
    }
}