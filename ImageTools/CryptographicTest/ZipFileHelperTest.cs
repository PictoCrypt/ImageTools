using FunctionLib.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CryptographicTest
{
    [TestClass]
    public class ZipFileHelperTest
    {
        [TestMethod]
        public void OpenZipTest()
        {
            var bytes = ZipFileHelper.ZipToBytes(Constants.Testzip);
            Assert.IsNotNull(bytes);
            Assert.IsTrue(bytes.Length > 0);
        }
    }
}
