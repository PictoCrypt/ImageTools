﻿using FunctionLib.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.CryptTests.Base;

namespace UnitTests.CryptTests
{
    [TestClass]
    public class TripleDesTest : SymmetricAlgorithmBaseTestClass
    {
        protected override CryptographicAlgorithmImpl Algorithm
        {
            get { return new TripleDesAlgorithm(); }
        }
    }
}