﻿using FunctionLib.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.CryptTests.Base;

namespace UnitTests.CryptTests
{
    [TestClass]
    public class Rc2Test : SymmetricAlgorithmBaseTestClass
    {
        protected override CryptographicAlgorithmImpl Algorithm
        {
            get { return new Rc2Algorithm(); }
        }
    }
}