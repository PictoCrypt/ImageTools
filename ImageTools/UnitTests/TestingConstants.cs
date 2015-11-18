using System.IO;
using FunctionLib.Helper;

namespace UnitTests
{
    public static class TestingConstants
    {
        public const string NormalText = "Ich bin der Testtext. " +
                                         "Dieser kann natürlich verändert werden. " +
                                         "Hier noch ein paar Sonderzeichen" +
                                         "!äü?ß][&_.\"§\"!§";

        public const string Password = "IchBinDasTestPasswort!";
        public const int LsbIndicator = 3;
        private static readonly string LoremIpsum = Path.Combine(Constants.ExecutiongPath, "Testfiles", "LoremIpsum.txt");
        public static readonly string LongText = File.ReadAllText(LoremIpsum);
        public static readonly string NormalImage = Path.Combine(Constants.ExecutiongPath, "Testfiles", "NormalTest.png");

        private static readonly string LittleImage = Path.Combine(Constants.ExecutiongPath, "Testfiles",
            "LittleTest.png");

        public static readonly string SmallKoala = Path.Combine(Constants.ExecutiongPath, "Testfiles", "SmallKoala.jpg");

        public static readonly string SmallFlowers = Path.Combine(Constants.ExecutiongPath, "Testfiles",
            "SmallFlowers.jpg");

        public static readonly string EncryptedImage = Path.Combine(Constants.ExecutiongPath, "Testfiles",
            "EncryptedTest.png");

        public static readonly string Testdoc = Path.Combine(Constants.ExecutiongPath, "Testfiles",
            "Testdoc.docx");
    }
}