using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using FunctionLib.Helper;
using FunctionLib.Model.Message;
using FunctionLib.Steganography;

namespace FunctionLib.Steganalyse
{
    /*
    *   This code is based on the DIIT steganography project, which is available at the following address: http://diit.sourceforge.net/
    */

    public class StegAnalyser
    {
        /**
         * Whether to run a Laplace Graph.
         */
        private readonly bool mRunLaplaceGraph;

        /**
         * Whether to run RS Analysis.
         */
        private readonly bool mRunRsAnalysis;

        /**
         * Whether to run Sample Pairs.
         */
        private readonly bool mRunSamplePairs;


        //VARIABLES

        /**
         * A string holding the results of all the steganalysis.
         */
        private string mResultsString;
        /**
        * Sets up steganalysis types that will be run.
        * 
        * @param runRsAnalysis Whether to run RS analysis.
        * @param runsamplepairs Whether to run Sample Pairs.
        * @param runlaplacegraph Whether to run a Laplace Graph.
        */

        public StegAnalyser(bool runRsAnalysis, bool runsamplepairs, bool runlaplacegraph)
        {
            mResultsString = "";
            mRunRsAnalysis = runRsAnalysis;
            mRunSamplePairs = runsamplepairs;
            mRunLaplaceGraph = runlaplacegraph;
        }

        //FUNCTIONS

        /**
         * Runs all the steganalysis.
         *
         * @param stego The stego image to test.  
         * @return All the results as text.
         * @throws IllegalArgumentException If the stego image is null.
         * @throws Exception If it has problems reading the images.
         */

        public string Run(Bitmap stego)
        {
            using (stego)
            {
                var image = new LockBitmap(stego);
                image.LockBits();
                var results = new StringBuilder("Results of steganalysis\n"
                                                + "==========================\n\n");

                string colour;
                double averageresults = 0, averagelength = 0;

                //RS Analysis
                if (mRunRsAnalysis)
                {
                    results.Append("RS ANALYSIS\n" + "============\n\n");
                    results.Append("RS Analysis (Non-overlapping groups)\n");
                    for (var j = 0; j < 3; j++)
                    {
                        var rsa = new RsAnalysis(2, 2);
                        var testresults =
                            rsa.DoAnalysis(image, j, false);

                        //get the right colour
                        if (j == 0)
                        {
                            colour = "red";
                        }
                        else if (j == 1)
                        {
                            colour = "green";
                        }
                        else
                        {
                            colour = "blue";
                        }

                        //Append the results
                        results.Append("Percentage in " + colour + ": ");

                        //Round and Append results
                        results.Append(Round(testresults[26]*100, 5) + "\n");

                        //and the approximate length (in bytes)
                        results.Append("Approximate length (in bytes) from " + colour + ": "
                                       + Round(testresults[27], 5) + "\n");

                        averageresults += testresults[26];
                        averagelength += testresults[27];
                    }

                    //now do again for overlapping groups
                    results.Append("\nRS Analysis (Overlapping groups)\n");
                    for (var j = 0; j < 3; j++)
                    {
                        var rsa = new RsAnalysis(2, 2);
                        var testresults =
                            rsa.DoAnalysis(image, j, true);

                        //get the right colour
                        if (j == 0)
                        {
                            colour = "red";
                        }
                        else if (j == 1)
                        {
                            colour = "green";
                        }
                        else
                        {
                            colour = "blue";
                        }

                        //Append the results
                        results.Append("Percentage in " + colour + ": ");

                        //Round and Append results
                        results.Append(Round(testresults[26]*100, 5) + "\n");

                        //and the approximate length (in bytes)
                        results.Append("Approximate length (in bytes) from " + colour + ": "
                                       + Round(testresults[27], 5) + "\n");

                        averageresults += testresults[26];
                        averagelength += testresults[27];
                    }

                    results.Append("\nAverage across all groups/colours: " +
                                   Round(averageresults/6*100, 5));
                    results.Append("\nAverage approximate length across all groups/colours: " +
                                   Round(averagelength/6, 5));
                    results.Append("\n\n\n");
                }


                //Sample Pairs
                averageresults = 0;
                averagelength = 0;
                if (mRunSamplePairs)
                {
                    results.Append("SAMPLE PAIRS\n" + "=============\n");
                    for (var j = 0; j < 3; j++)
                    {
                        var sp = new SamplePairs();
                        var estimatedlength = sp.DoAnalysis(image, j);
                        var numbytes = image.Height*image.Width*3/8
                                       *estimatedlength;

                        //get the right colour
                        if (j == 0)
                        {
                            colour = "red";
                        }
                        else if (j == 1)
                        {
                            colour = "green";
                        }
                        else
                        {
                            colour = "blue";
                        }

                        //Append the results
                        results.Append("Percentage in " + colour + ": ");

                        //Round and Append results
                        results.Append(Round(estimatedlength*100, 5) + "\n");

                        //and the approximate length (in bytes)
                        results.Append("Approximate length (in bytes) from " + colour + ": "
                                       + Round(numbytes, 5) + "\n");

                        averageresults += estimatedlength;
                        averagelength += numbytes;
                    }

                    //average results
                    results.Append("\nAverage across all groups/colours: " +
                                   Round(averageresults/3*100, 5));
                    results.Append("\nAverage approximate length across all groups/colours: " +
                                   Round(averagelength/3, 5));
                    results.Append("\n\n\n");
                }


                //Laplace graph
                if (mRunLaplaceGraph)
                {
                    results.Append("LAPLACE GRAPH (CSV formatted)\n"
                                   + "==============================\n\n");
                    results.Append(LaplaceGraph.GetCSVGraph(image));
                }

                //Append some new lines to make it look nice
                results.Append("\n\n\n\n");

                mResultsString = results.ToString();
                image.UnlockBits();
            }
            return mResultsString;
        }


        /**
             * Combines two directories into an output directory.
             * <P>
             * The output directory (tempdir) will have all the original
             * images plus all the stegoimages copied into it.
             * 
             * @param messagedir The directory full of messages in (txt files).
             * @param imagedir The directory full of images (png, jpg, bmp).
             * @param tempdir The directory to write the results to.
             * @param algorithm The algorithm to use to combine the messages and
             * images with.
             * @throws IllegalArgumentException If any directories are null.
             * @return A string full of skipped files.
             */

        public string CreateCombineDirectories(string messagedir, string imagedir, string tempdir,
            SteganographicAlgorithmImpl algorithm)
        {
            if (!Directory.Exists(messagedir) || !Directory.Exists(imagedir) || !Directory.Exists(tempdir))
            {
                throw new ArgumentException("Must be directories!");
            }

            var errors = new StringBuilder("Errors: \n========\n\n");
            var messageList = Directory.GetFiles(messagedir);
            var imagelist = Directory.GetFiles(imagedir);
            string coverfilepath, messagefilepath, outputpath, originalname;
            var fileseperator = Path.DirectorySeparatorChar;
            //	//for each file in the image folder...
            foreach (var imagefile in imagelist)
            {
                if (imagefile.EndsWith(".bmp") || imagefile.EndsWith(".jpg") || imagefile.EndsWith(".png"))
                {
                    //ok to combine it...
                    coverfilepath = imagedir;
                    originalname = imagefile.Substring(0, imagefile.IndexOf("."));
                    //for each message in the message folder...
                    foreach (var messagefile in messageList)
                    {
                        try
                        {
                            if (messagefile.EndsWith(".txt"))
                            {
                                //ok to combine...
                                messagefilepath = messagefile;

                                //setup the two files...

                                //setup the filename...
                                outputpath = originalname + "~" + messagefile.Substring(0, messagefile.LastIndexOf(".")) +
                                             "-"
                                             +
                                             algorithm.GetType()
                                                 .Name.ToLower()
                                                 .Substring(algorithm.GetType().Name.LastIndexOf(".") + 1,
                                                     algorithm.GetType().Name.Length) + "." + "png";

                                //write it
                                new Bitmap(algorithm.Encode(coverfilepath,
                                    new TextMessage(File.ReadAllText(messagefilepath)), 0)).Save(tempdir);
                            }
                        }
                        catch (Exception)
                        {
                            //just go onto the next one...
                            errors.Append("Error: Could not process: " + imagefile + " with " + messagefile + "\n");
                        }
                        //end image loop
                        //cleanup to speed up memory
                        GC.Collect();
                    }
                }
            }

            return errors.ToString();
        }


        /**
     * Generates a CSV formatted string of steganalysis information.
     * <P>
     * The directory passed has all it's image files steganalysed and
     * the results are returned in a comma separated file format. No spaces
     * are used in column headings, and all bar the steganography type are
     * numerical values.
     * 
     * @param directory The directory to steganalyse.
     * @param laplacelimit The number of laplace values to write out in total.
     * @return A string containing a csv file of results.
     */

        public string GetCSV(string directory, int laplacelimit)
        {
            //output progress to console
            Console.WriteLine("\n\nCSV Progress: {");
            var files = Directory.GetFiles(directory);
            var fivepercent = (int) Math.Floor((double) files.Length/20);

            var csv = new StringBuilder();

            //add all the headings
            if (mRunRsAnalysis)
            {
                var rsa = new RsAnalysis(2, 2);
                var rflag = "(rs overlapping)";
                string colour;

                //overlapping
                for (var i = 0; i < 3; i++)
                {
                    IEnumerable<string> rnames = rsa.GetResultNames();
                    //get the right colour
                    if (i == 0)
                    {
                        colour = " red ";
                    }
                    else if (i == 1)
                    {
                        colour = " green ";
                    }
                    else
                    {
                        colour = " blue ";
                    }
                    foreach (var rname in rnames)
                    {
                        var aname = rname;
                        var towrite = aname + colour + rflag + ",";
                        towrite = towrite.Replace(' ', '-');
                        csv.Append(towrite);
                    }
                }

                //non overlapping
                rflag = "(rs non-overlapping)";
                for (var i = 0; i < 3; i++)
                {
                    IEnumerable<string> rnames = rsa.GetResultNames();
                    //get the right colour
                    if (i == 0)
                    {
                        colour = " red ";
                    }
                    else if (i == 1)
                    {
                        colour = " green ";
                    }
                    else
                    {
                        colour = " blue ";
                    }
                    foreach (var rname in rnames)
                    {
                        var aname = rname;
                        var towrite = aname + colour + rflag + ",";
                        towrite = towrite.Replace(' ', '-');
                        csv.Append(towrite);
                    }
                }
            }
            if (mRunSamplePairs)
            {
                string colour;

                //overlapping
                for (var i = 0; i < 3; i++)
                {
                    //get the right colour
                    if (i == 0)
                    {
                        colour = "-red-";
                    }
                    else if (i == 1)
                    {
                        colour = "-green-";
                    }
                    else
                    {
                        colour = "-blue-";
                    }
                    csv.Append("SP-Percentage" + colour + ",");
                    csv.Append("SP-Approximate-Bytes" + colour + ",");
                }
            }
            if (mRunLaplaceGraph)
            {
                for (var i = 0; i < laplacelimit; i++)
                {
                    csv.Append("Laplace-value-" + i + ",");
                }
            }
            csv.Append("Steganography-Type,Image-Name\n");


            //check all the files
            for (var i = 0; i < files.Length; i++)
            {
                //print progress
                if (i > 0 && fivepercent > 0)
                {
                    if (i%fivepercent == 0)
                    {
                        Console.WriteLine("#");
                    }
                }

                if (files[i].EndsWith(".bmp") || files[i].EndsWith(".png")
                    || files[i].EndsWith(".jpg"))
                {
                    //file can be worked on.


                    string flag;

                    try
                    {
                        using (var bitmap = new Bitmap(files[i]))
                        {
                            var image = new LockBitmap(bitmap);
                            image.LockBits();

                            //run RS analysis
                            if (mRunRsAnalysis)
                            {
                                //overlapping
                                for (var j = 0; j < 3; j++)
                                {
                                    var rsa = new RsAnalysis(2, 2);
                                    var testresults =
                                        rsa.DoAnalysis(image, j, true);

                                    for (var k = 0; k < testresults.Length; k++)
                                    {
                                        csv.Append(testresults[k] + ",");
                                    }
                                }
                                //non-overlapping
                                for (var j = 0; j < 3; j++)
                                {
                                    var rsa = new RsAnalysis(2, 2);
                                    var testresults =
                                        rsa.DoAnalysis(image, j, false);

                                    for (var k = 0; k < testresults.Length; k++)
                                    {
                                        csv.Append(testresults[k] + ",");
                                    }
                                }
                            }

                            //run Sample Pairs
                            if (mRunSamplePairs)
                            {
                                //overlapping
                                for (var j = 0; j < 3; j++)
                                {
                                    var sp = new SamplePairs();
                                    var estimatedlength = sp.DoAnalysis(image, j);
                                    var numbytes = image.Height*image.Width*3/8
                                                   *estimatedlength;
                                    csv.Append(estimatedlength + "," + numbytes + ",");
                                }
                            }

                            //run LaplaceGraph
                            if (mRunLaplaceGraph)
                            {
                                var lgres = LaplaceGraph.GetGraph(image);

                                for (var j = 0; j < laplacelimit; j++)
                                {
                                    if (lgres.Length <= laplacelimit && j >= lgres.Length)
                                    {
                                        csv.Append("0,");
                                    }
                                    else
                                    {
                                        if (lgres[j][0] != j)
                                        {
                                            csv.Append("0,");
                                        }
                                        else
                                        {
                                            csv.Append(lgres[j][1] + ",");
                                        }
                                    }
                                }
                            }

                            if (files[i].IndexOf("_") >= 0 || files[i].IndexOf("-") >= 0)
                            {
                                if (files[i].IndexOf("_") >= 0)
                                {
                                    flag = files[i].Substring(files[i].IndexOf("_") + 1, files[i].LastIndexOf("."));
                                }
                                else
                                {
                                    flag = files[i].Substring(files[i].IndexOf("-") + 1, files[i].LastIndexOf("."));
                                }
                            }
                            else
                            {
                                flag = "none";
                            }

                            csv.Append(flag);
                            //Append in the file name
                            csv.Append("," + files[i]);

                            if (csv[csv.Length - 1] == ',')
                            {
                                csv.Remove(csv.Length - 1, 1);
                            }

                            csv.Append("\n");
                            image.UnlockBits();
                        }
                    }
                    catch (Exception)
                    {
                        //skip the file...
                    }
                    //cleanup to speed up memory
                    GC.Collect();
                }
            }

            //all done
            Console.WriteLine("} Complete!");

            csv.Append("\n");
            return csv.ToString();
        }


/**
 * Creates an ARFF file of steganography information.
 * <P>
 * An ARFF file is the natural internal format for WEKA - Waikato
 * Environment for Knowledge Analysis.  WEKA can also handle CSV
 * files but it is much nicer to be able to produce the natural format.
 * The same information as per the CSV generator is produced here, just
 * in a different format.
 * 
 * @param directory The directory to steganalyse.
 * @param laplacelimit The maximum number of laplace values to output.
 * @param relationname The internal name of the relation as it will be
 * seen in WEKA.
 * @return An ARFF formatted file full of the steganalysis information.
 * @see www.cs.waikato.ac.nz/ml/weka
 * 
 */

        public string GetARFF(string directory, int laplacelimit, string relationname)
        {
            var arff = new StringBuilder();

            //output progress to console
            Console.WriteLine("\n\nARFF Progress: {");
            var files = Directory.GetFiles(directory);
            var fivepercent = (int) Math.Floor((double) files.Length/20);


            arff.Append("% Steganography Benchmarking Data\n%\n");
            arff.Append("% Sourced from automatic generation in Digital Invisible Ink Toolkit\n");
            arff.Append("% Generator created by Kathryn Hempstalk.\n");
            arff.Append("% Generator copyright under the Gnu General Public License, 2005\n");
            arff.Append("\n");

            arff.Append("\n@relation '" + relationname + "'\n\n");


            //add all the headings
            if (mRunRsAnalysis)
            {
                var rsa = new RsAnalysis(2, 2);
                var rflag = "(rs overlapping)";
                string colour;

                //overlapping
                for (var i = 0; i < 3; i++)
                {
                    IEnumerable<string> rnames = rsa.GetResultNames();
                    //get the right colour
                    if (i == 0)
                    {
                        colour = " red ";
                    }
                    else if (i == 1)
                    {
                        colour = " green ";
                    }
                    else
                    {
                        colour = " blue ";
                    }
                    foreach (var rname in rnames)
                    {
                        var aname = rname;
                        var towrite = aname + colour + rflag;
                        arff.Append("@attribute '" + towrite + "' numeric\n");
                    }
                }

                //non overlapping
                rflag = "(rs non-overlapping)";
                for (var i = 0; i < 3; i++)
                {
                    IEnumerable<string> rnames = rsa.GetResultNames();
                    //get the right colour
                    if (i == 0)
                    {
                        colour = " red ";
                    }
                    else if (i == 1)
                    {
                        colour = " green ";
                    }
                    else
                    {
                        colour = " blue ";
                    }
                    foreach (var rname in rnames)
                    {
                        var aname = rname;
                        var towrite = aname + colour + rflag;
                        arff.Append("@attribute '" + towrite + "' numeric\n");
                    }
                }
            }
            if (mRunSamplePairs)
            {
                string colour;

                //overlapping
                for (var i = 0; i < 3; i++)
                {
                    //get the right colour
                    if (i == 0)
                    {
                        colour = " red ";
                    }
                    else if (i == 1)
                    {
                        colour = " green ";
                    }
                    else
                    {
                        colour = " blue ";
                    }
                    arff.Append("@attribute 'SP Percentage" + colour + "' numeric\n");
                    arff.Append("@attribute 'SP Approximate Bytes" + colour + "' numeric\n");
                }
            }
            if (mRunLaplaceGraph)
            {
                for (var i = 0; i < laplacelimit; i++)
                {
                    arff.Append("@attribute 'Laplace value " + i + "' numeric\n");
                }
            }


            arff.Append("@attribute 'Steganography Type' {");
            //iterate through all the hashmap values...
            var stegotypes = GetStegTypes(Directory.GetFiles(directory));
            var valuesarray = stegotypes.Values.ToArray();
            arff.Append(valuesarray[0]);
            for (var i = 1; i < valuesarray.Length; i++)
            {
                arff.Append("," + valuesarray[i]);
            }
            arff.Append("}\n");
            arff.Append("@attribute 'Image Name' string\n");


            arff.Append("\n@data\n");


            //check all the files
            for (var i = 0; i < files.Length; i++)
            {
                //print progress
                if (i > 0 && fivepercent > 0)
                {
                    if (i%fivepercent == 0 && i != 0)
                    {
                        Console.WriteLine("#");
                    }
                }

                if (files[i].EndsWith(".bmp") || files[i].EndsWith(".png")
                    || files[i].EndsWith(".jpg"))
                {
                    //file can be worked on.


                    try
                    {
                        using (var bitmap = new Bitmap(files[i]))
                        {
                            var image = new LockBitmap(bitmap);
                            image.LockBits();

                            //run RS analysis
                            if (mRunRsAnalysis)
                            {
                                //overlapping
                                for (var j = 0; j < 3; j++)
                                {
                                    var rsa = new RsAnalysis(2, 2);
                                    var testresults =
                                        rsa.DoAnalysis(image, j, true);

                                    for (var k = 0; k < testresults.Length; k++)
                                    {
                                        arff.Append(testresults[k] + ",");
                                    }
                                }
                                //non-overlapping
                                for (var j = 0; j < 3; j++)
                                {
                                    var rsa = new RsAnalysis(2, 2);
                                    var testresults =
                                        rsa.DoAnalysis(image, j, false);

                                    for (var k = 0; k < testresults.Length; k++)
                                    {
                                        arff.Append(testresults[k] + ",");
                                    }
                                }
                            }

                            //run Sample Pairs
                            if (mRunSamplePairs)
                            {
                                //overlapping
                                for (var j = 0; j < 3; j++)
                                {
                                    var sp = new SamplePairs();
                                    var estimatedlength = sp.DoAnalysis(image, j);
                                    var numbytes = image.Height*image.Width*3/8
                                                   *(estimatedlength/100);
                                    arff.Append(estimatedlength + "," + numbytes + ",");
                                }
                            }

                            //run LaplaceGraph
                            if (mRunLaplaceGraph)
                            {
                                var lgres = LaplaceGraph.GetGraph(image);

                                for (var j = 0; j < laplacelimit; j++)
                                {
                                    if (lgres.Length <= laplacelimit && j >= lgres.Length)
                                    {
                                        arff.Append("0,");
                                    }
                                    else
                                    {
                                        if (lgres[j][0] != j)
                                        {
                                            arff.Append("0,");
                                        }
                                        else
                                        {
                                            arff.Append(lgres[j][1] + ",");
                                        }
                                    }
                                }
                            }

                            string flag;
                            if (files[i].IndexOf("_") >= 0 || files[i].IndexOf("-") >= 0)
                            {
                                if (files[i].IndexOf("_") >= 0)
                                {
                                    flag = files[i].Substring(files[i].IndexOf("_") + 1, files[i].LastIndexOf("."));
                                }
                                else
                                {
                                    flag = files[i].Substring(files[i].IndexOf("-") + 1, files[i].LastIndexOf("."));
                                }
                            }
                            else
                            {
                                flag = "none";
                            }

                            arff.Append(flag);
                            arff.Append("," + files[i]);

                            if (arff[arff.Length - 1] == ',')
                            {
                                arff.Remove(arff.Length - 1, 1);
                            }

                            arff.Append("\n");
                            image.UnlockBits();
                        }
                    }
                    catch (Exception)
                    {
                        //skip the file...
                    }
                    //cleanup to speed up memory
                    GC.Collect();
                }
            }

            //all done
            Console.WriteLine("} Complete!");

            return arff.ToString();
        }


/**
 * Gets a map of all the steganography types in a 
 * given list of files.
 * <P>
 * A steganography type is the word between an underscore or hyphen and
 * the final dot in the file. None is added by default.  If there is no
 * hyphen/underscore then no steg type will be added.
 *
 * @param filelist The list of files.
 * @return A hashmap full of steganography names.
 */

        private Dictionary<string, string> GetStegTypes(string[] files)
        {
            var stegotypes = new Dictionary<string, string>();
            stegotypes.Add("none", "none");
            var stegtype = "";
            for (var i = 0; i < files.Length; i++)
            {
                if (files[i].EndsWith(".bmp") || files[i].EndsWith(".png"))
                {
                    //also add in the stego type
                    if (files[i].IndexOf("_") >= 0)
                    {
                        //steg type detected...
                        stegtype = files[i].Substring
                            (files[i].IndexOf("_") + 1, files[i].LastIndexOf("."));
                        stegotypes.Add(stegtype, stegtype);
                    }
                    else if (files[i].IndexOf("-") >= 0)
                    {
                        //steg type detected...
                        stegtype = files[i].Substring
                            (files[i].IndexOf("-") + 1, files[i].LastIndexOf("."));
                        stegotypes.Add(stegtype, stegtype);
                    }
                }
            }

            return stegotypes;
        }


        /**
         * Returns the last results of this steg analyser.
         *
         * @return The last results.
         */

        public override string ToString()
        {
            return mResultsString;
        }


        /**
         * Rounds to the specified number of decimal places.
         *
         * @param number The number to Round.
         * @param places The number of decimal places to Round to.
         * @return The Rounded number.
         */

        public double Round(double number, int places)
        {
            var multiple = Math.Pow(10, places);
            number = number*multiple;
            var num2 = Math.Round(number);
            number = num2/multiple;

            if (number < 0.000000000001)
            {
                return 0;
            }
            return number;
        }
    }
}