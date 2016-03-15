﻿using System.Security.Cryptography;

namespace FunctionLib.Cryptography
{
    public class BlowfishAlgorithm : CryptographicAlgorithmImpl
    {
        protected override SymmetricAlgorithm Algorithm
        {
            get { return new Blowfish.Blowfish(); }
        }
    }
}