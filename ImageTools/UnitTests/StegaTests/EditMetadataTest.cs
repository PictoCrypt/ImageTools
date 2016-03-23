using FunctionLib.Steganography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.StegaTests
{
    [TestClass]
    public class EditMetadataTest : SteganographicAlogithmBaseTestClass
    {
        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
            Algorithm = new EditMetadata();
        }
    }
}