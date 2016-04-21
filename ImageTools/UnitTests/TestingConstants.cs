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
        private static readonly string LoremIpsum = Path.Combine(Constants.ExecutiongPath, "Testfiles", "LoremIpsum.txt");
        public static readonly string LongText = File.ReadAllText(LoremIpsum);
        public static readonly string NormalImage = Path.Combine(Constants.ExecutiongPath, "Testfiles", "NormalTest.png");

        public static readonly string NormalJpgImage = Path.Combine(Constants.ExecutiongPath, "Testfiles",
            "NormalTest.jpg");


        public static readonly string SmallImage = Path.Combine(Constants.ExecutiongPath, "Testfiles", "SmallImage.jpg");

        public static readonly string SmallJpgImage = Path.Combine(Constants.ExecutiongPath, "Testfiles",
            "SmallImage.png");

        public static readonly string NormalTestdoc = Path.Combine(Constants.ExecutiongPath, "Testfiles",
            "NormalTestdoc.docx");

        public static readonly string LargeTestdoc = Path.Combine(Constants.ExecutiongPath, "Testfiles",
            "LargeTestdoc.docx");
    }
}