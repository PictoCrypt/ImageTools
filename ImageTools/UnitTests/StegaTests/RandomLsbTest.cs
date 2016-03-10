﻿using System;
using System.Drawing;
using FunctionLib.Model.Message;
using FunctionLib.Steganography.Base;
using FunctionLib.Steganography.LSB;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.StegaTests
{
    [TestClass]
    public class RandomLsbTest : SteganographicAlogithmBaseTestClass
    {
        protected override Bitmap Encode(Bitmap src, ISecretMessage value, int password, int additionalParam = 3)
        {
            return SteganographicAlgorithmBase.Encode(this, typeof(RandomLsb), src, value, password,
                additionalParam);
        }

        protected override ISecretMessage Decode(Bitmap src, int password, MessageType type, int additionalParam = 3)
        {
            return SteganographicAlgorithmBase.Decode(this, typeof(RandomLsb), src, password, type, additionalParam);
        }
    }
}