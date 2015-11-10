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
        private static readonly string NormalImage = Path.Combine(MethodHelper.ExecutiongPath, "Testfiles", "NormalTest.png");
        private static readonly string LittleImage = Path.Combine(MethodHelper.ExecutiongPath, "Testfiles", "LittleTest.png");
        private static readonly string SmallKoala = Path.Combine(MethodHelper.ExecutiongPath, "Testfiles", "SmallKoala.jpg");
        private static readonly string SmallFlowers = Path.Combine(MethodHelper.ExecutiongPath, "Testfiles", "SmallFlowers.jpg");
        public static readonly string EncryptedImage = Path.Combine(MethodHelper.ExecutiongPath, "Testfiles", "EncryptedTest.png");
        public static readonly Bitmap NormalBitmap = new Bitmap(NormalImage);
        public static readonly Bitmap SmallKoalaImage = new Bitmap(SmallKoala);
        public static readonly Bitmap SmallFlowersImage = new Bitmap(SmallFlowers);
        public static readonly object LittleBitmap = new Bitmap(LittleImage);
        public static readonly string Testzip = Path.Combine(MethodHelper.ExecutiongPath, "Testfiles",
            "TestZip.zip");

    }
}