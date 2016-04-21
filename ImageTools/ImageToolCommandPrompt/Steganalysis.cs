using System;
using System.Drawing;
using System.IO;
using FunctionLib.Steganalyse;

namespace ImageToolCommandPrompt
{
    public class Steganalysis : CommandTool
    {
        private string mImagePath;
        private bool mLaplacianGraph;
        private bool mRsAnalysis;
        private bool mSamplePair;

        public Steganalysis(string[] args) : base(args)
        {
        }

        protected override string Name
        {
            get { return "STEGANALYSIS"; }
        }

        protected override bool Configured
        {
            get
            {
                if (File.Exists(mImagePath))
                {
                    if (mSamplePair || mRsAnalysis || mLaplacianGraph)
                    {
                        return true;
                    }
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
                        mImagePath = parameter;
                    }
                    break;
                case "s":
                    mSamplePair = true;
                    break;
                case "r":
                    mRsAnalysis = true;
                    break;
                case "l":
                    mLaplacianGraph = true;
                    break;
                default:
                    Console.WriteLine("Argument {0} does not exist.", map);
                    break;
            }
        }

        protected override void RunSetup()
        {
            InitializeSetup();
            Console.WriteLine("Image path:");
            mImagePath = Console.ReadLine();
            if (!File.Exists(mImagePath))
            {
                WriteError("File does not exist: '" + mImagePath + "'");
            }

            Console.WriteLine("Run Sample Pair Analysis? Y/N");
            mSamplePair = GetBool(Console.ReadLine());

            Console.WriteLine("Run RS Analysis? Y/N");
            mRsAnalysis = GetBool(Console.ReadLine());

            Console.WriteLine("Run Laplacian Graph Analysis? Y/N");
            mLaplacianGraph = GetBool(Console.ReadLine());

            RunWithParameters();
        }

        protected override void RunWithParameters()
        {
            if (Configured)
            {
                InitializeRun();
                string result;
                var analyzer = new StegAnalyser(mRsAnalysis, mSamplePair, mLaplacianGraph);
                using (var bitmap = new Bitmap(mImagePath))
                {
                    result = analyzer.Run(bitmap);
                }
                Console.WriteLine(result);
            }
        }

        public override void Help()
        {
            InitializeHelp();
            Console.WriteLine("-i Image path");
            Console.WriteLine("-s Sample pair analysis on/off; default is off");
            Console.WriteLine("-r RS analysis on/off; default is off");
            Console.WriteLine("-l laplacian graph analysis on/off; default is off");
            Console.WriteLine("]");
        }
    }
}