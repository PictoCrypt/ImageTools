namespace FunctionLib.Model
{
    public class Pixel
    {
        public Pixel(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }

        public override bool Equals(object obj)
        {
            var other = obj as Pixel;
            if (other == null)
            {
                return false;
            }
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode()*Y.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("({0} | {1})", X, Y);
        }
    }
}