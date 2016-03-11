using System;

namespace FunctionLib.CustomException
{
    public class UniqueNumberException : Exception
    {
        public UniqueNumberException(string message) : base(message)
        {
        }
    }
}