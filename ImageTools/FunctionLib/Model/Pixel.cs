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

        public override string ToString()
        {
            return string.Format("({0} | {1})", X, Y);
        }
    }
}