using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace FunctionLib.Cryptography
{
    public abstract class CryptographicAlgorithmImpl
    {
        protected const int Iterations = 2;
        protected const string Salt = "jasdh7834y8hfeur73rsharks214"; // Random
        protected const string Vector = "8947az34awl34kjq"; // Random
        protected static int mKeySize = 256;

        public abstract string Name { get; }

        public string Encode(string value, string password)
        {
            var valueBytes = Encoding.UTF8.GetBytes(value);
            var result = Encode(valueBytes, password);
            return Convert.ToBase64String(result);
        }

        public abstract byte[] Encode(byte[] value, string password);

        public virtual string Decode(string value, string password)
        {
            var valueBytes = Convert.FromBase64String(value);
            var bytes = Decode(valueBytes, password);
            var result = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            return result;
        }

        public abstract byte[] Decode(byte[] value, string password);

        public override string ToString()
        {
            return Name;
        }

        public override int GetHashCode()
        {
            var result = Name.GetHashCode() + Iterations.GetHashCode() + Salt.GetHashCode() + Vector.GetHashCode();
            return result;
        }

        public override bool Equals(object obj)
        {
            var other = obj as CryptographicAlgorithmImpl;
            if (other == null)
            {
                return false;
            }

            if (Name.Equals(other.Name))
            {
                return true;
            }
            return false;
        }
    }
}