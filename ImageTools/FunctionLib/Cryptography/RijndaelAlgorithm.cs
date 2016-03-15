﻿using System.Security.Cryptography;

namespace FunctionLib.Cryptography
{
    public class RijndaelAlgorithm : CryptographicAlgorithmImpl
    {
        protected override SymmetricAlgorithm Algorithm
        {
            get { return new RijndaelManaged(); }
        }
    }
}