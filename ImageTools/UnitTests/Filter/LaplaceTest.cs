using System;
using System.Collections.Generic;
using System.Drawing;
using FunctionLib.Filter;
using FunctionLib.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Filter
{
    [TestClass]
    public class LaplaceTest
    {
        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void LaplaceTestWithoutImage()
        {
            var filter = new Laplace(null as LockBitmap, 1, 8);
            var result = filter.GetValue(0, 0);
        }

        [TestMethod]
        public void LaplaceTestWithSampleImage()
        {
            var image = new Bitmap(TestingConstants.SmallFlowers);
            var filter = new Laplace(image, 1, 8);
            var results = new List<int>();
            for (var x = 0; x < image.Width; x++)
            {
                for (var y = 0; y < image.Height; y++)
                {
                    results.Add(filter.GetValue(x, y));
                }
            }

            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count > 0);
            Assert.IsTrue(results.Count == image.Width*image.Height);
        }
    }
}