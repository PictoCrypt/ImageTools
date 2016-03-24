﻿using System;
using System.Drawing;
using FunctionLib.Filter;
using FunctionLib.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.FilterTests
{
    [TestClass]
    public class SobelTest : FilterTest
    {
        protected override Filter GenerateFilter(Bitmap image, int startbits, int endbits)
        {
            return new Sobel(image, startbits, endbits);
        }
    }
}
