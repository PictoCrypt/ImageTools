﻿using System.Drawing;
using FunctionLib.Filter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.FilterTests
{
    [TestClass]
    public class PrewittTest : FilterTest
    {
        protected override Filter GenerateFilter(Bitmap image, int startbits, int endbits)
        {
            return new Prewitt(image, startbits, endbits);
        }
    }
}