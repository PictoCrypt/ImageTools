using System.Drawing;
using FunctionLib.Steganalyse;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.FunctionLibTests
{
    [TestClass]
    public class AnalysisTest
    {
        [TestMethod]
        public void SamplePairAnalysisTest()
        {
            string result;
            using (var stego = new Bitmap(TestingConstants.NormalJpgImage))
            {
                var analyser = new StegAnalyser(false, true, false);
                result = analyser.Run(stego);
            }
            Assert.IsFalse(string.IsNullOrEmpty(result));
        }

        [TestMethod]
        public void RsAnalysisTest()
        {
            string result;
            using (var stego = new Bitmap(TestingConstants.NormalJpgImage))
            {
                var analyser = new StegAnalyser(true, false, false);
                result = analyser.Run(stego);
            }
            Assert.IsFalse(string.IsNullOrEmpty(result));
        }


        [TestMethod]
        public void LaplaceGraphAnalysisTest()
        {
            string result;
            using (var stego = new Bitmap(TestingConstants.NormalJpgImage))
            {
                var analyser = new StegAnalyser(false, false, true);
                result = analyser.Run(stego);
            }
            Assert.IsFalse(string.IsNullOrEmpty(result));
        }
    }
}