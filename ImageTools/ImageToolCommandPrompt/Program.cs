using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FunctionLib.Cryptography;
using FunctionLib.Helper;
using FunctionLib.Model;
using FunctionLib.Steganography.Base;

namespace ImageToolCommandPrompt
{
    public class Program
    {
        private static string mSource;
        private static string mMessage;
        private static int mCryptIndex;
        private static string mPassword;
        private static int mSteganoIndex;
        private static bool mCompression;
        private static int mLsbIndicator = 3;
        private static string mResultpath;

        public static void Main(string[] args)
        {
            if (args.Length == 0 || args.Contains("HELP", StringComparer.OrdinalIgnoreCase))
            {
                Console.WriteLine(Seperator + " Help " + Seperator);
                Console.WriteLine();
                Console.WriteLine(Seperator + " ENCODING " + Seperator);
                Console.WriteLine("ENCODING \t Starts the encoding setup without parameters.");
                Console.WriteLine("ENCODING [");
                Console.WriteLine("-s Sourcepath ");
                Console.WriteLine("-m Message or Path to file ");
                Console.WriteLine("-c Index of Cryptographic Algorithm ");
                Console.WriteLine("-p Password ");
                Console.WriteLine("-l Index of Steganographic Algorithm");
                Console.WriteLine("-r Resultpath");
                Console.WriteLine("[-k Compression on/off; default is off]");
                Console.WriteLine("[-i Least Significant Bit indicator; default is 3]");

                Console.WriteLine();
                Console.WriteLine(Seperator + " ENCODING " + Seperator);
                Console.WriteLine("DECODING \t Starts the encoding setup without parameters.");
                Console.WriteLine("DECODING [");
                Console.WriteLine("-s Sourcepath ");
                Console.WriteLine("-m Message or Path to file ");
                Console.WriteLine("-c Index of Cryptographic Algorithm ");
                Console.WriteLine("-p Password ");
                Console.WriteLine("-l Index of Steganographic Algorithm");
                Console.WriteLine("-r Resultpath");
                Console.WriteLine("[-k Compression on/off; default is off]");
                Console.WriteLine("[-i Least Significant Bit indicator; default is 3]");
            }

            if (args.Length == 1)
            {
                if (args.Contains("ENCODING", StringComparer.OrdinalIgnoreCase))
                {
                    Encode();
                }
                if (args.Contains("DECODING", StringComparer.OrdinalIgnoreCase))
                {
                    Decode();
                }
            }
            else
            {
                var function = args[0];
                args = args.Skip(1).ToArray();
                for (var i = 0; i < args.Count(x => x.StartsWith("-")); i++)
                {
                    switch (args[i++])
                    {
                        case "-s":
                            mSource = args[i];
                            break;
                        case "-m":
                            mMessage = args[i];
                            break;
                        case "-c":
                            mCryptIndex = int.Parse(args[i]);
                            break;
                        case "-p":
                            mPassword = args[i];
                            break;
                        case "-l":
                            mSteganoIndex = int.Parse(args[i]);
                            break;
                        case "-r":
                            mResultpath = args[i];
                            break;
                        case "-k":
                            mCompression = bool.Parse(args[i]);
                            break;
                        case "-i":
                            mLsbIndicator = int.Parse(args[i]);
                            break;

                        default:
                            WriteError("Unregonized argument: " + args[i]);
                            break;
                    }
                }
                if (function.Equals("ENCODE", StringComparison.OrdinalIgnoreCase))
                {
                    EncodeWithParamters();
                }
                else if(function.Equals("DECODE", StringComparison.OrdinalIgnoreCase))
                {
                    DecodeWithParameters();
                }
            }
            
            
            //TODO maybe show changed pixels?
        }

        private static void DecodeWithParameters()
        {
            var model = new DecodeModel(mSource, GetCrypt(mCryptIndex), mPassword, GetStego(mSteganoIndex), mCompression, mLsbIndicator);
            var result = model.Decode();
            using (var sw = new StreamWriter(File.Create(mResultpath)))
            {
                sw.Write(result);
            }
        }

        private static void EncodeWithParamters()
        {
            var model = new EncodeModel(mSource, mMessage, GetCrypt(mCryptIndex), mPassword, GetStego(mSteganoIndex), mCompression, mLsbIndicator);
            var result = model.Encode();
            using (result)
            {
                result.Save(mResultpath);
            }
        }

        private static CryptographicAlgorithmImpl GetCrypt(int index)
        {
            var algorithms = AlgorithmCollector.GetAllAlgorithm<CryptographicAlgorithmImpl>();
            return algorithms[index];
        }

        private static SteganographicAlgorithmImpl GetStego(int index)
        {
            var algorithms = AlgorithmCollector.GetAllAlgorithm<SteganographicAlgorithmImpl>();
            return algorithms[index];
        }

        private const string Seperator = "--------------------";

        public static void Encode()
        {
            Console.WriteLine(Seperator + " SETUP ENCODING " + Seperator);
            Console.WriteLine("Path to source image: ");
            var source = Console.ReadLine();
            if (!File.Exists(source))
            {
                WriteError(string.Format("Path {0} is no valid file.", source));
            }

            Console.WriteLine("Path to file or text message:");
            var message = Console.ReadLine();
            
            var crypto = CryptographicAlgorithmChooser();
            string password = string.Empty;
            if (crypto != null)
            {
                Console.WriteLine("Password:");
                password = Console.ReadLine();
                if (string.IsNullOrEmpty(password))
                {
                    WriteError("Password cannot be empty.");
                }
            }
            var stegano = SteganographicAlgorithmChooser();

            Console.WriteLine("Is Compression required? Y/N");
            var compression = Console.ReadLine().Equals("Y", StringComparison.OrdinalIgnoreCase);

            Console.WriteLine("How much bits should be replaced? Minimum is 1, maximum 8 for full replacement.");
            var lsbIndicator = int.Parse(Console.ReadLine());

            EncodeModel model = null;
            try
            {
                model = new EncodeModel(source, message, crypto, password, stegano, compression, lsbIndicator);
            }
            catch (Exception ex)
            {
                WriteError(ex.Message);
            }

            Console.WriteLine("Press <ENTER> to start encoding.");
            Console.ReadLine();
            Console.WriteLine(Seperator + " ENCODING " + Seperator);
            //TODO Show process?
            var result = model.Encode();
            Console.WriteLine();
            if (result != null)
            {
                Console.WriteLine("ENCODING successfully. Please enter a path to save:");
                var path = Console.ReadLine();
                using (result)
                {
                    result.Save(path);
                }
                Console.WriteLine("Image saved at " + path);
            }

            Console.WriteLine("Press <ENTER> to exit.");
            Console.ReadLine();
        }

        public static void Decode()
        {
            Console.WriteLine(Seperator + " SETUP DECODING " + Seperator);
            Console.WriteLine("Path to source image: ");
            var source = Console.ReadLine();
            if (!File.Exists(source))
            {
                WriteError(string.Format("Path {0} is no valid file.", source));
            }
            var crypto = CryptographicAlgorithmChooser();
            string password = "";
            if (crypto != null)
            {
                Console.WriteLine("Password:");
                password = Console.ReadLine();
                if (string.IsNullOrEmpty(password))
                {
                    WriteError("Password cannot be empty.");
                }
            }
            var stegano = SteganographicAlgorithmChooser();

            Console.WriteLine("Is Compression required? Y/N");
            var compression = Console.ReadLine().Equals("Y", StringComparison.OrdinalIgnoreCase);

            Console.WriteLine("How much bits should be replaced? Minimum is 1, maximum 8 for full replacement.");
            var lsbIndicator = int.Parse(Console.ReadLine());

            DecodeModel model = null;
            try
            {
                model = new DecodeModel(source, crypto, password, stegano, compression, lsbIndicator);
            }
            catch (Exception ex)
            {
                WriteError(ex.Message);
            }

            Console.WriteLine("Press <ENTER> to start decoding.");
            Console.ReadLine();
            Console.WriteLine(Seperator + " DECODING " + Seperator);
            //TODO Show process?
            var result = model.Decode();
            Console.WriteLine();
            if (!string.IsNullOrEmpty(result))
            {
                Console.WriteLine("DECODING successfully. Please enter a path to save:");
                Console.WriteLine("Message or Filepath: " + result);
            }

            Console.WriteLine("Press <ENTER> to exit.");
            Console.ReadLine();

        }

        private static SteganographicAlgorithmImpl SteganographicAlgorithmChooser()
        {
            Console.WriteLine(Seperator +  " Steganographic Algorithm Chooser " + Seperator);
            var algorithms = AlgorithmCollector.GetAllAlgorithm<SteganographicAlgorithmImpl>();
            return algorithms[AlgorithmChooser(algorithms.Select(x => x.Name))];
        }

        private static CryptographicAlgorithmImpl CryptographicAlgorithmChooser()
        {
            Console.WriteLine(Seperator + " Cryptographic Algorithm Chooser " + Seperator);
            var algorithms = AlgorithmCollector.GetAllAlgorithm<CryptographicAlgorithmImpl>();
            return algorithms[AlgorithmChooser(algorithms.Select(x => x.Name))];
        }

        private static int AlgorithmChooser(IEnumerable<string> algorithms)
        {
            Console.WriteLine("{0} \t {1}", "Index", "Algorithm Name");
            int index = 0;
            foreach (var algorithm in algorithms)
            {
                Console.WriteLine("{0} \t {1}", index++, algorithm);
            }
            Console.WriteLine("Please choose an algorithm by typing the index:");
            index = int.Parse(Console.ReadLine());
            return index;
        }

        private static void WriteError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();

            //TODO better error handling?
            Environment.Exit(0xA0);
        }
    }
}
