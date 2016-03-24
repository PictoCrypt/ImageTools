using System;
using System.Collections.Generic;
using System.Drawing;
using FunctionLib.Filter;
using FunctionLib.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.FilterTests
{
    public abstract class FilterTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FilterTestWithoutImage()
        {
            var filter = GenerateFilter(null, 1, 8);
            var result = filter.GetValue(0, 0);
        }

        [TestMethod]
        public void NormalFilterTest()
        {
            using (var image = new Bitmap(TestingConstants.SmallImage))
            {
                var filter = GenerateFilter(image, 1, 8);
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
                Assert.IsTrue(results.Count == image.Width * image.Height);
            }
        }

        protected abstract Filter GenerateFilter(Bitmap image, int startbits, int endbits);
    }
}