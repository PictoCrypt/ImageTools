using System;
using System.Collections.Generic;
using FunctionLib.CustomException;

namespace FunctionLib.Steganography.LSB
{
    public abstract class LsbWithRandomness : LsbAlgorithmBase
    {
        private readonly HashSet<int> mXCoordinates = new HashSet<int>();
        private readonly HashSet<int> mYCoordinates = new HashSet<int>();

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
                    return result;
                case Coordinate.Y:
                    result = random.Next(value);
                    while (mYCoordinates.Contains(result))
                    {
                        result = random.Next(value);
                    }
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