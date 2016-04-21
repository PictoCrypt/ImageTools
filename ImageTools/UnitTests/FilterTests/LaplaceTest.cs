using System.Drawing;
using FunctionLib.Filter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.FilterTests
{
    [TestClass]
    public class LaplaceTest : FilterTest
    {
        protected override Filter GenerateFilter(Bitmap image, int startbits, int endbits)
        {
            return new Laplace(image, startbits, endbits);
        }
    }
}