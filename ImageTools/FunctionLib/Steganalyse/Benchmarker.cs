using System;
using System.Drawing;

namespace FunctionLib.Steganalyse
{
    /*
    *   This code is based on the DIIT steganography project, which is available at the following address: http://diit.sourceforge.net/
    */
    public class Benchmarker
    {
//VARIABLES

/**
 * A string holding the results of all the benchmarking tests.
 */
        private string mResultsString;

/**
 * Whether to run an Average Absolute Difference benchmark.
 */
        private readonly bool mRunAverageAbsoluteDifference;

/**
 * Whether to run a Correlation Quality benchmark.
 */
        private readonly bool mRunCorrelationQuality;

/**
 * Whether to run a Laplacian Mean Squared Error benchmark.
 */
        private readonly bool mRunLaplacianMeanSquaredError;

/**
 * Whether to run a LpNorm benchmark.
 */
        private readonly bool mRunLpNorm;

/**
 * Whether to run a Mean Squared Error benchmark.
 */
        private readonly bool mRunMeanSquaredError;

/**
 * Whether to run a Normalised Cross-Correlation benchmark.
 */
        private readonly bool mRunNormalisedCrossCorrelation;

/**
 * Whether to run a Peak Signal-to-Noise Ratio benchmark.
 */
        private readonly bool mRunPeakSignalToNoiseRatio;

/**
 * Whether to run a Signal-to-Noise Ratio benchmark.
 */
        private readonly bool mRunSignalToNoiseRatio;
        /*
         * Sets up the benchmarker ready for use.  All tests are set as
         * per the entered values.
         * 
         * @param runaadiff Whether to run Average Absolute Difference.
         * @param runmserror Whether to run Mean Squared Error.
         * @param runlpnorm Whether to run Lp Norm.
         * @param runlpmserror Whether to run Laplacian Mean Squared Error.
         * @param runsnr Whether to run Signal to Noise Ratio.
         * @param runpeaksnr Whether to run Peak Signal to Noise Ratio.
         * @param runncc Whether to run Normalised Cross Correlation.
         * @param runcquality Whether to run Correlation Quality.
         */

        public Benchmarker(bool runaadiff, bool runmserror,
            bool runlpnorm, bool runlpmserror,
            bool runsnr, bool runpeaksnr,
            bool runncc, bool runcquality)
        {
            mResultsString = "";
            mRunAverageAbsoluteDifference = runaadiff;
            mRunMeanSquaredError = runmserror;
            mRunLpNorm = runlpnorm;
            mRunLaplacianMeanSquaredError = runlpmserror;
            mRunSignalToNoiseRatio = runsnr;
            mRunPeakSignalToNoiseRatio = runpeaksnr;
            mRunNormalisedCrossCorrelation = runncc;
            mRunCorrelationQuality = runcquality;
        }

        //FUNCTIONS

        /**
         * Runs all the benchmarking tests.
         *
         * @param original The original image to compare against. 
         * @param stego The stego image to test.  
         * @return All the results as text.
         * @throws IllegalArgumentException If the stego image is null.
         * @throws Exception If it has problems reading the images.
         */

        public string Run(Bitmap original, Bitmap stego)
        {
            if (original == null)
                throw new ArgumentNullException(nameof(original));
            if (stego == null)
                throw new ArgumentNullException(nameof(stego));

            //setup temp variables
            Analysis bench;
            mResultsString = "Results of benchmark tests\n"
                             + "==========================\n\n";


            //run all the tests...
            if (mRunAverageAbsoluteDifference)
            {
                bench = new AverageAbsoluteDifference();
                mResultsString = mResultsString
                                 + bench
                                 + ": "
                                 + bench.Calculate(original, stego)
                                 + "\n";
            }
            if (mRunMeanSquaredError)
            {
                bench = new MeanSquaredError();
                mResultsString = mResultsString
                                 + bench
                                 + ": "
                                 + bench.Calculate(original, stego)
                                 + "\n";
            }
            if (mRunLpNorm)
            {
                bench = new LpNorm();
                mResultsString = mResultsString
                                 + bench
                                 + ": "
                                 + bench.Calculate(original, stego)
                                 + "\n";
            }
            if (mRunLaplacianMeanSquaredError)
            {
                bench = new LaplacianMeanSquaredError();
                mResultsString = mResultsString
                                 + bench
                                 + ": "
                                 + bench.Calculate(original, stego)
                                 + "\n";
            }
            if (mRunSignalToNoiseRatio)
            {
                bench = new SignalToNoiseRatio();
                mResultsString = mResultsString
                                 + bench
                                 + ": "
                                 + bench.Calculate(original, stego)
                                 + "\n";
            }
            if (mRunPeakSignalToNoiseRatio)
            {
                bench = new PeakSignalToNoiseRatio();
                mResultsString = mResultsString
                                 + bench
                                 + ": "
                                 + bench.Calculate(original, stego)
                                 + "\n";
            }
            if (mRunNormalisedCrossCorrelation)
            {
                bench = new NormalizedCrossCorrelation();
                mResultsString = mResultsString
                                 + bench
                                 + ": "
                                 + bench.Calculate(original, stego)
                                 + "\n";
            }
            if (mRunCorrelationQuality)
            {
                bench = new CorrelationQuality();
                mResultsString = mResultsString
                                 + bench
                                 + ": "
                                 + bench.Calculate(original, stego)
                                 + "\n";
            }


            return mResultsString;
        }


        /*
	     * Returns the last results of this benchmarker.
	     *
	     * @return The last results.
	     */
        public override string ToString()
        {
            return mResultsString;
        }

        /*
         * Rounds to the specified number of decimal places.
         *
         * @param number The number to round.
         * @param places The number of decimal places to round to.
         * @return The rounded number.
         */

        public double Round(double number, int places)
        {
            var multiple = Math.Pow(10, places);
            number = number*multiple;
            var num2 = Math.Round(number);
            number = num2/multiple;

            if (number < 0.000000000001)
                return 0;
            return number;
        }
    }
}