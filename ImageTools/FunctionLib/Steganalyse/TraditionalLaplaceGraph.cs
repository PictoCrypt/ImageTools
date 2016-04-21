using System;
using System.Drawing;
using System.Text;
using FunctionLib.Filter;

namespace FunctionLib.Steganalyse
{
    /*
    *   This code is based on the DIIT steganography project, which is available at the following address: http://diit.sourceforge.net/
    */

    public class TraditionalLaplaceGraph
    {
        /**
         * Outputs a laplace graph in csv format.
         *
         * The CSV file will be comma separated, and is 
         * written to the specified place on disk.
         *
         * @param image The image to create the laplace graph of.
         * @return A string representation of the laplace graph in
         * the format of CSV.
         */

        public static string GetCSVGraph(Bitmap image)
        {
            var sb = new StringBuilder();
            sb.Append("\"Frequency\",\"Laplace Value\"\n");
            var graph = GetGraph(image);
            for (var i = 0; i < graph.Length; i++)
            {
                sb.Append(graph[i][1] + "," + graph[i][0] + "\n");
            }

            sb.Append("\n\n");
            return sb.ToString();
        }

        /**
	 * Gets the traditional laplace graph of an image.  The image
	 * is assumed to be able to be translated into black and white.
	 * The values in the graph can be negative.
	 * 
	 * @param image The image to get the graph of.
	 * @return The graph of the image.
	 */

        public static double[][] GetGraph(Bitmap image)
        {
            var filter = new TraditionalLaplace(image, 0, 8);

            //filter the image
            var fparray =
                new FilteredPixel[image.Width*image.Height];
            for (var i = 0; i < image.Width; i++)
            {
                for (var j = 0; j < image.Height; j++)
                {
                    fparray[i*image.Height + j] =
                        new FilteredPixel(i, j,
                            filter.GetValue(i, j));
                }
            }
            //sort the filter results
            //is in ascending order - low at start, high at end
            Array.Sort(fparray, new FpComparator());

            //now for each individual filter result, we count how many we have

            //first find out how many different values we have
            var numdistinct = 1;
            for (var i = 1; i < fparray.Length; i++)
            {
                if (fparray[i].FilterValue != fparray[i - 1].FilterValue)
                    numdistinct++;
            }

            //now we create an array to hold the filter values and their counts
            var results = new double[numdistinct][];
            //var results = new double[numdistinct][2];
            results[0][0] = fparray[0].FilterValue;
            results[0][1] = 1;
            var k = 0;

            //now we fill up the array
            foreach (var t in fparray)
            {
                if (results[k][0] != t.FilterValue)
                {
                    k++;
                    results[k][0] = t.FilterValue;
                    results[k][1] = 1;
                }
                else
                {
                    results[k][1]++;
                }
            }

            //now normalise the graph
            foreach (var t in results)
            {
                t[1] = t[1]/fparray.Length;
            }

            //graph produced, return results
            return results;
        }
    }
}