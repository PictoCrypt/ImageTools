using System;
using System.Security.Cryptography;

namespace FunctionLib.Helper
{
    public static class PasswordHelper
    {
        public static int GetHash(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            //TODO: Bessere Methode?
            return password.GetHashCode();
        }
    }
}