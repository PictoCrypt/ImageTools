using System.Drawing;
using System.IO;
using FunctionLib.Helper;

namespace CryptographicTest
{
    public static class Constants
    {
        public const string NormalText = "Ich bin der Testtext. " +
                                         "Dieser kann natürlich verändert werden. " +
                                         "Hier noch ein paar Sonderzeichen" +
                                         "!äü?ß][&_.\"§\"!§";

        public const string Password = "IchBinDasTestPasswort!";
        public const int NormalAdditionalParam = 3;
        private static readonly string LoremIpsum = Path.Combine(MethodHelper.ExecutiongPath, "Testfiles", "LoremIpsum.txt");
        public static readonly string LongText = File.ReadAllText(LoremIpsum);
        public static readonly string NormalImage = Path.Combine(MethodHelper.ExecutiongPath, "Testfiles", "NormalTest.png");
        public static readonly string EncryptedImage = Path.Combine(MethodHelper.ExecutiongPath, "Testfiles", "EncryptedTest.png");
        public static readonly Bitmap NormalBitmap = new Bitmap(NormalImage);
        public static readonly string Testzip = Path.Combine(MethodHelper.ExecutiongPath, "Testfiles",
            "TestZip.zip");
    }
}