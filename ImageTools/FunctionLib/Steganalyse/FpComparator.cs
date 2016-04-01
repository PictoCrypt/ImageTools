using System.Collections;

namespace FunctionLib.Steganalyse
{
    /*
    *   This code is based on the DIIT steganography project, which is available at the following address: http://diit.sourceforge.net/
    */

    public class FpComparator : IComparer
    {
        /**
        * Compares two objects to see if they are similar.
        *
        * A pixel is first compared by it's filter value.  To prevent deadlock
        * pixels with the same value, the x and y positions are used.  Therefore
        * to get a value of zero, the pixels must be in the same positions, as
        * well as having the same filter value.
        * 
        * @see java.util.Comparator
        * @param o1 The first object to be compared.
        * @param o2 The second object to be compared.
        * @return 0 if they are the same, 
        * negative if o2 is bigger, positive if o1 is bigger.
        */

        public int Compare(object o1, object o2)
        {
            var a = (FilteredPixel) o1;
            var b = (FilteredPixel) o2;

            //break a tie
            if (a.FilterValue == b.FilterValue)
            {
                if (a.X == b.X)
                {
                    if (a.Y == b.Y)
                    {
                        return 0;
                    }
                    if (a.Y > b.Y)
                    {
                        //bigger y value, make it "bigger"
                        return 20;
                    }
                    return -20;
                }
                if (a.Y > b.Y)
                {
                    //bigger x value, make it "bigger"
                    return 50;
                }
                return -50;
            }
            if (a.FilterValue < b.FilterValue)
            {
                return -100;
            }
            return 100;
        }
    }
}