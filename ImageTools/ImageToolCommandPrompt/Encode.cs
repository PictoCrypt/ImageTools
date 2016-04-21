using System;
using System.IO;
using FunctionLib.Cryptography;
using FunctionLib.Model;
using FunctionLib.Steganography;

namespace ImageToolCommandPrompt
{
    public class Encode : CommandTool
    {
        private bool mCompression;
        private int mCryptIndex;

        private string mImagePath;
        private int mLsbIndicator;
        private string mMessage;
        private string mPassword;
        private string mResultPath;
        private int mStegoIndex;

        public Encode(string[] args) : base(args)
        {
        }

        protected override string Name
        {
            get { return "ENCODING"; }
        }

        protected override bool Configured
        {
            get
            {
                return File.Exists(mImagePath) && !string.IsNullOrEmpty(mMessage) && !string.IsNullOrEmpty(mResultPath) &&
                       mStegoIndex >= 0;
            }
        }

        protected override void MapArgument(string map, string parameter)
        {
            switch (map)
            {
                case "-s":
                    if (!File.Exists(parameter))
                    {
                        WriteError("File does not exist: '" + parameter + "'");
                    }
                    else
                    {
                        mImagePath = parameter;
                    }
                    break;
                case "-m":
                    mMessage = parameter;
                    break;
                case "-c":
                    mCryptIndex = int.Parse(parameter);
                    break;
                case "-p":
                    mPassword = parameter;
                    break;
                case "-l":
                    mStegoIndex = int.Parse(parameter);
                    break;
                case "-r":
                    mResultPath = parameter;
                    break;
                case "-k":
                    mCompression = bool.Parse(parameter);
                    break;
                case "-i":
                    mLsbIndicator = int.Parse(parameter);
                    break;

                default:
                    Console.WriteLine("Argument {0} does not exist.", map);
                    break;
            }
        }

        protected override void RunSetup()
        {
            InitializeSetup();
            Console.WriteLine("Path to source image: ");
            mImagePath = Console.ReadLine();
            if (!File.Exists(mImagePath))
            {
                WriteError("File does not exist: '" + mImagePath + "'");
            }

            Console.WriteLine("Path to file or text message:");
            mMessage = Console.ReadLine();

            var crypto = AlgorithmChooser<CryptographicAlgorithmImpl>();
            if (crypto >= 0)
            {
                Console.WriteLine("Password:");
                mPassword = Console.ReadLine();
                if (string.IsNullOrEmpty(mPassword))
                {
                    WriteError("Password cannot be empty.");
                }
            }
            var stegano = AlgorithmChooser<SteganographicAlgorithmImpl>();

            Console.WriteLine("Is Compression required? Y/N");
            mCompression = Console.ReadLine().Equals("Y", StringComparison.OrdinalIgnoreCase);

            Console.WriteLine("How much bits should be replaced? Minimum is 1, maximum 8 for full replacement.");
            mLsbIndicator = int.Parse(Console.ReadLine());

            Console.WriteLine("Path to result: ");
            mResultPath = Console.ReadLine();
        }

        protected override void RunWithParameters()
        {
            if (Configured)
            {
                InitializeRun();
                //TODO Show process?
                var model = new EncodeModel(mImagePath, mMessage, GetCrypt(mCryptIndex), mPassword,
                    GetStego(mStegoIndex), mCompression, mLsbIndicator);
                var result = model.Encode();
                Console.WriteLine();
                if (result != null)
                {
                    Console.WriteLine("ENCODING successfully. Please enter a path to save:");
                    var path = Console.ReadLine();
                    File.Move(result, mResultPath);
                    Console.WriteLine("Image saved at " + path);
                }

                Console.WriteLine("Press <ENTER> to exit.");
                Console.ReadLine();
            }
        }


        public override void Help()
        {
            InitializeHelp();
            Console.WriteLine("-s Sourcepath ");
            Console.WriteLine("-m Message or Path to file ");
            Console.WriteLine("-c Index of Cryptographic Algorithm ");
            Console.WriteLine("-p Password ");
            Console.WriteLine("-l Index of Steganographic Algorithm");
            Console.WriteLine("-r Resultpath");
            Console.WriteLine("[-k Compression on/off; default is off]");
            Console.WriteLine("[-i Least Significant Bit indicator; default is 3]");
            Console.WriteLine("]");
        }
    }
}