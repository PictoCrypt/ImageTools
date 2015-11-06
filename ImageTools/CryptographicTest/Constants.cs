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
        private static readonly string LoremIpsum = Path.Combine(MethodHelper.ExecutiongPath, "LoremIpsum.txt");
        public static readonly string LongText = File.ReadAllText(LoremIpsum);
        public static readonly string NormalImage = Path.Combine(MethodHelper.ExecutiongPath, "NormalTest.png");
        public static readonly string EncryptedImage = Path.Combine(MethodHelper.ExecutiongPath, "EncryptedTest.png");
        public static readonly Bitmap NormalBitmap = new Bitmap(NormalImage);
        public const int NormalAdditionalParam = 3;
    }
}