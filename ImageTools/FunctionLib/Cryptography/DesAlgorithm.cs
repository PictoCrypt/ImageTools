﻿using System.Security.Cryptography;

namespace FunctionLib.Cryptography
{
    public class DesAlgorithm : CryptographicSymmetricAlgorithmImpl
    {
        protected override SymmetricAlgorithm Algorithm
        {
            get { return new DESCryptoServiceProvider(); }
        }

        public override string Name
        {
            get { return "DES"; }
        }
    }
}