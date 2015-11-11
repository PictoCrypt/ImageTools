﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using FunctionLib.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.StegaTests
{
    public abstract class SteganographicAlogithmBaseTestClass
    {
        private TimeSpan mDecryptionTime;
        private TimeSpan mEncryptionTime;
        private Stopwatch mStopwatch;

        [TestInitialize]
        public void Initialize()
        {
            mStopwatch = new Stopwatch();
        }

        [TestMethod]
        public void NormalEncryptionTest()
        {
            var encrypted = Encrypt(TestingConstants.NormalBitmap, TestingConstants.NormalText);

            var decrypted = Decrypt(encrypted);

            Assert.IsTrue(decrypted.ToString().StartsWith(TestingConstants.NormalText));

            WriteToOutput();
        }

        [TestMethod]
        //[ExpectedException(typeof(ArgumentOutOfRangeException))]
        [ExpectedException(typeof (TargetInvocationException))]
        public void EncryptImageWithNotEnoughSpace()
        {
            const int lsbIndicator = 4;
            var encrypted = Encrypt(TestingConstants.SmallKoalaImage, TestingConstants.SmallFlowersImage, lsbIndicator);
        }

        [TestMethod]
        public void EncryptImageTest()
        {
            const int lsbIndicator = 4;
            var encrypted = Encrypt(TestingConstants.NormalBitmap, TestingConstants.SmallFlowersImage, lsbIndicator);

            var decrypted = new Bitmap(Decrypt(encrypted, lsbIndicator).ToString());

            Assert.IsNotNull(decrypted);
            Assert.IsTrue(TestingConstants.SmallFlowersImage.Size == decrypted.Size);

            WriteToOutput();
        }

        [TestMethod]
        [ExpectedException(typeof (TargetInvocationException))]
        public void EncryptionWithoutTextTest()
        {
            var encrypted = Encrypt(TestingConstants.NormalBitmap, string.Empty);
        }

        private object Decrypt(Bitmap encrypted)
        {
            mStopwatch.Start();
            var decrypted = Decrypt(encrypted, TestingConstants.NormalAdditionalParam);
            mStopwatch.Stop();
            mDecryptionTime = mStopwatch.Elapsed;
            Assert.IsFalse(decrypted == null);
            return decrypted;
        }

        private Bitmap Encrypt(Bitmap src, object value)
        {
            mStopwatch.Start();
            var encrypted = Encrypt(src, value, TestingConstants.NormalAdditionalParam);
            mStopwatch.Stop();
            mEncryptionTime = mStopwatch.Elapsed;
            mStopwatch.Reset();
            Assert.IsTrue(encrypted != null);
            return encrypted;
        }

        private void WriteToOutput()
        {
            Trace.WriteLine(GetType() + "- TEST");
            Trace.WriteLine("Encryption-Time: " + mEncryptionTime);
            Trace.WriteLine("Decryption-Time: " + mDecryptionTime);
            Trace.WriteLine("");
            Trace.WriteLine("");
        }

        public abstract Bitmap Encrypt(Bitmap src, object value, int additionalParam);
        public abstract object Decrypt(Bitmap src, int additionalParam);
    }
}