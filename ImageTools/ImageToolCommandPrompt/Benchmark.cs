using System;
using System.Drawing;
using System.IO;
using FunctionLib.Steganalyse;

namespace ImageToolCommandPrompt
{
    public class Benchmark : CommandTool
    {
        private string mOriginalPath;
        private string mSteganogrammPath;

        public Benchmark()
        {
        }

        public Benchmark(string[] args) : base(args)
        {
        }

        protected override string Name
        {
            get { return "BENCHMARK"; }
        }

        protected override bool Configured
        {
            get
            {
                if (File.Exists(mOriginalPath) && File.Exists(mSteganogrammPath))
                {
                    return true;
                }
                return false;
            }
        }

        protected override void MapArgument(string map, string parameter)
        {
            switch (map)
            {
                case "i":
                    if (!File.Exists(parameter))
                    {
                        WriteError("File does not exist: '" + parameter + "'");
                    }
                    else
                    {
                        mOriginalPath = parameter;
                    }
                    break;
                case "s":
                    if (!File.Exists(parameter))
                    {
                        WriteError("File does not exist: '" + parameter + "'");
                    }
                    else
                    {
                        mSteganogrammPath = parameter;
                    }
                    break;
                default:
                    Console.WriteLine("Argument {0} does not exist.", map);
                    break;
            }
        }

        protected override void RunSetup()
        {
            InitializeSetup();
            Console.WriteLine("Original image path:");
            mOriginalPath = Console.ReadLine();
            if (!File.Exists(mOriginalPath))
            {
                WriteError("File does not exist: '" + mOriginalPath + "'");
            }

            Console.WriteLine("Steganogramm path:");
            mSteganogrammPath = Console.ReadLine();
            if (!File.Exists(mSteganogrammPath))
            {
                WriteError("File does not exist: '" + mSteganogrammPath + "'");
            }


            RunWithParameters();
        }

        protected override void RunWithParameters()
        {
            if (Configured)
            {
                InitializeRun();
                string result;
                var benchmarker = new Benchmarker(true, true, true, true, true, true, true, true);

                using (var original = new Bitmap(mOriginalPath))
                {
                    using (var stego = new Bitmap(mSteganogrammPath))
                    {
                        result = benchmarker.Run(original, stego);
                    }
                }
                Console.WriteLine(result);
            }
        }

        public override void Help()
        {
            InitializeHelp();
            Console.WriteLine("-i Original image path");
            Console.WriteLine("-s Steganogramm path");
            Console.WriteLine("]");
        }
    }
}