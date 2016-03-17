using System;
using System.Collections.Generic;
using System.Drawing;
using FunctionLib.CustomException;
using FunctionLib.Model.Message;

namespace FunctionLib.Steganography.LSB
{
    public abstract class LsbWithRandomness : LsbAlgorithmBase
    {
        private HashSet<int> mXCoordinates;
        private HashSet<int> mYCoordinates;
        protected Random Random { get; set; }

        protected override void InitializeEncoding(Bitmap src, ISecretMessage message, int passHash)
        {
            base.InitializeEncoding(src, message, passHash);
            mXCoordinates = new HashSet<int>();
            mYCoordinates = new HashSet<int>();
            Random = new Random(PassHash);
        }

        protected override void InitializeDecoding(Bitmap src, int passHash)
        {
            base.InitializeDecoding(src, passHash);
            mXCoordinates = new HashSet<int>();
            mYCoordinates = new HashSet<int>();
            Random = new Random(PassHash);
        }

        protected int GetNextRandom(Coordinate coordinate, int value, Random random)
        {
            int result;
            switch (coordinate)
            {
                case Coordinate.X:
                    result = random.Next(value);
                    while (mXCoordinates.Contains(result))
                    {
                        result = random.Next(value);
                    }
                    mXCoordinates.Add(result);
                    return result;
                case Coordinate.Y:
                    result = random.Next(value);
                    while (mYCoordinates.Contains(result))
                    {
                        result = random.Next(value);
                    }
                    mYCoordinates.Add(result);
                    return result;
            }
            throw new UniqueNumberException("Error generating unique random number.");
        }
    }

    public enum Coordinate
    {
        X,
        Y
    }
}