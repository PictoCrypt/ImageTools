using System;
using System.Collections.Generic;
using System.Linq;
using FunctionLib.Helper;

namespace ImageToolCommandPrompt
{
    public static class Program
    {
        private static List<CommandTool> StaticCommandTools
        {
            get { return AlgorithmCollector.GetAllAlgorithm<CommandTool>(); }
        }

        public static void Main(string[] args)
        {
            Console.WriteLine(CommandTool.Seperator + " HELP " + CommandTool.Seperator);
            if (args == null || args.Length == 0 || args[0].Equals("HELP", StringComparison.OrdinalIgnoreCase))
            {
                WriteHelp();
            }
            else if (args.Length > 0)
            {
                var key = args[0].ToUpperInvariant();
                args = args.Skip(1).ToArray();
                switch (key)
                {
                    case "ENCODING":
                        var encode = new Encode(args);
                        break;
                    case "DECODING":
                        var decode = new Decode(args);
                        break;
                    case "STEGANALYSIS":
                        var steganalysis = new Steganalysis(args);
                        break;
                    case "BENCHMARK":
                        var benchmark = new Benchmark(args);
                        break;
                    default:
                        WriteHelp();
                        break;
                }
            }
        }

        private static void WriteHelp()
        {
            foreach (var commandTool in StaticCommandTools)
            {
                commandTool.Help();
                Console.ReadLine();
            }
        }
    }
}