using System;
using System.Drawing;
using System.IO;

namespace FunctionLib.Steganalyse
{
    /*
    *   This code is based on the DIIT steganography project, which is available at the following address: http://diit.sourceforge.net/
    */
    /**
     * Sample pairs analysis for an image.
     * <P>
     * Sample pairs analysis is a technique for detecting
     * steganography in an image.  More information can be found
     * in the paper "Detection of LSB steganography via Sample Pair analysis".
     * This implementation is based off some C++ code kindly provided by 
     * the authors of the paper.
     *
     * @author Kathryn Hempstalk
     */
    public class SamplePairs
    {
        /**
         * Does sample pairs analysis on an image.
         *
         * @param image The image to analyse.
         */
        public double DoAnalysis(Bitmap image, int colour)
        {

            //get the images sizes
            int imgx = image.Width, imgy = image.Height;

            int startx = 0, starty = 0;
            var apair = new Color[2];
            int u, v;
            long P, X, Y, Z;
            long W;

            P = X = Y = Z = W = 0;

            //pairs across the image
            for (starty = 0; starty < imgy; starty++)
            {
                for (startx = 0; startx < imgx; startx = startx + 2)
                {
                    //get the block of data (2 pixels)
                    apair[0] = image.GetPixel(startx, starty);
                    apair[1] = image.GetPixel(startx + 1, starty);

                    u = GetPixelColour(apair[0], colour);
                    v = GetPixelColour(apair[1], colour);


                    //if the 7 msb are the same, but the 1 lsb are different
                    if ((u >> 1 == v >> 1) && ((v & 0x1) != (u & 0x1)))
                        W++;
                    //if the pixels are the same
                    if (u == v)
                        Z++;
                    //if lsb(v) = 0 & u < v OR lsb(v) = 1 & u > v
                    if ((v == (v >> 1) << 1) && (u < v) || (v != (v >> 1) << 1) && (u > v))
                        X++;
                    //vice versa
                    if ((v == (v >> 1) << 1) && (u > v) || (v != (v >> 1) << 1) && (u < v))
                        Y++;
                    P++;
                }
            }

            //pairs down the image
            for (starty = 0; starty < imgy; starty = starty + 2)
            {
                for (startx = 0; startx < imgx; startx++)
                {

                    //get the block of data (2 pixels)
                    apair[0] = image.GetPixel(startx, starty);
                    apair[1] = image.GetPixel(startx, starty + 1);

                    u = GetPixelColour(apair[0], colour);
                    v = GetPixelColour(apair[1], colour);

                    //if the 7 msb are the same, but the 1 lsb are different
                    if ((u >> 1 == v >> 1) && ((v & 0x1) != (u & 0x1)))
                        W++;
                    //the pixels are the same
                    if (u == v)
                        Z++;
                    //if lsb(v) = 0 & u < v OR lsb(v) = 1 & u > v
                    if ((v == (v >> 1) << 1) && (u < v) || (v != (v >> 1) << 1) && (u > v))
                        X++;
                    //vice versa
                    if ((v == (v >> 1) << 1) && (u > v) || (v != (v >> 1) << 1) && (u < v))
                        Y++;
                    P++;
                }
            }

            //solve the quadratic equation
            //in the form ax^2 + bx + c = 0
            double a = 0.5 * (W + Z);
            double b = 2 * X - P;
            double c = Y - X;

            //the result
            double x;

            //straight line
            if (a == 0)
                x = c / b;

            //curve
            //take it as a curve
            double discriminant = Math.Pow(b, 2) - (4 * a * c);

            if (discriminant >= 0)
            {
                double rootpos = ((-1 * b) + Math.Sqrt(discriminant)) / (2 * a);
                double rootneg = ((-1 * b) - Math.Sqrt(discriminant)) / (2 * a);

                //return the root with the smallest absolute value (as per paper)
                if (Math.Abs(rootpos) <= Math.Abs(rootneg))
                    x = rootpos;
                else
                    x = rootneg;
            }
            else {
                x = c / b;
            }

            if (x == 0)
            {
                //let's assume straight lines again, something is probably wrong
                x = c / b;
            }

            return x;
        }





        /**
         * Gets the given colour value for this pixel.
         * 
         * @param pixel The pixel to get the colour of.
         * @param colour The colour to get.
         * @return The colour value of the given colour in the given pixel.
         */
        public int GetPixelColour(Color pixel, int colour)
        {
            if (colour == RsAnalysis.ANALYSIS_COLOUR_RED)
                return pixel.R;
            else if (colour == RsAnalysis.ANALYSIS_COLOUR_GREEN)
                return pixel.G;
            else if (colour == RsAnalysis.ANALYSIS_COLOUR_BLUE)
                return pixel.B;
            else
                return 0;
        }


        /*
         * A small main method that will print out the message length
         * in percent of pixels.
         */
        public static void Main(String[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: invisibleinktoolkit.benchmark.SamplePairs <imagefilename>");
                Environment.Exit(1);
            }
            try
            {
                Console.WriteLine("\nSample Pairs Results");
                Console.WriteLine("--------------------");
                SamplePairs sp = new SamplePairs();
                Bitmap image = new Bitmap(args[0]);
                double average = 0;
                double results = sp.DoAnalysis(image, SamplePairs.ANALYSIS_COLOUR_RED);
                Console.WriteLine("Result from red: " + results);
                average += results;
                results = sp.DoAnalysis(image, SamplePairs.ANALYSIS_COLOUR_GREEN);
                Console.WriteLine("Result from green: " + results);
                average += results;
                results = sp.DoAnalysis(image, SamplePairs.ANALYSIS_COLOUR_BLUE);
                Console.WriteLine("Result from blue: " + results);
                average += results;
                average = average / 3;
                Console.WriteLine("Average result: " + average);
                Console.WriteLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: Cannot process that image type, please try another image.");
                Console.WriteLine(e.StackTrace);
            }
        }


        //VARIABLES


        /**
         * Denotes analysis to be done with red.
         */
        public const int ANALYSIS_COLOUR_RED = 0;

        /**
         * Denotes analysis to be done with green.
         */
        public const int ANALYSIS_COLOUR_GREEN = 1;

        /**
         * Denotes analysis to be done with blue.
         */
        public const int ANALYSIS_COLOUR_BLUE = 2;
    }
}