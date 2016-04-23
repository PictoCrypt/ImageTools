using FunctionLib.Steganography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.StegaTests
{
    [TestClass]
    public class EditMetadataTest : SteganographicAlogithmBaseTestClass
    {
        protected override SteganographicAlgorithmImpl Algorithm { get {return new EditMetadata();} }
    }
}