﻿using System.Security.Cryptography;

namespace FunctionLib.Cryptography
{
    public class DesAlgorithm : CryptographicAlgorithmImpl
    {
        protected override SymmetricAlgorithm Algorithm
        {
            get { return new DESCryptoServiceProvider(); }
        }
    }
}