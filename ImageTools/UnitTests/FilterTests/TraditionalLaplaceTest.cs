using System.Drawing;
using FunctionLib.Filter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.FilterTests
{
    [TestClass]
    public class TraditionalLaplaceTest : FilterTest
    {
        protected override Filter GenerateFilter(Bitmap image, int startbits, int endbits)
        {
            return new TraditionalLaplace(image, startbits, endbits);
        }
    }
}
