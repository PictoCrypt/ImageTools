namespace FunctionLib.Steganalyse
{
    /*
    *   This code is based on the DIIT steganography project, which is available at the following address: http://diit.sourceforge.net/
    */

    public class FilteredPixel
    {
        public FilteredPixel(int x, int y, int value)
        {
            X = x;
            Y = y;
            FilterValue = value;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int FilterValue { get; set; }

        public override string ToString()
        {
            return string.Format("X: {0} Y: {1}", X, Y);
        }

        /**
         * Compares two objects to see if they are equal.
         *
         * 
         * @see java.util.Comparator
         * @param o1 The first object to be compared.
         * @param o2 The second object to be compared.
         * @return True if they are equal, false otherwise.
         */

        public override bool Equals(object obj)
        {
            var a = this;
            var b = (FilteredPixel) obj;
            if (b == null)
            {
                return false;
            }
            return a.FilterValue == b.FilterValue &&
                   a.X == b.X && a.Y == b.Y;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode() + FilterValue.GetHashCode();
        }
    }
}