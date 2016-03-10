namespace FunctionLib.CustomException
{
    public class UniqueNumberException : System.Exception
    {
        public UniqueNumberException(string message) : base(message)
        {
        }
    }
}