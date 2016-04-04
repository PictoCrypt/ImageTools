using System.Collections.Generic;
using System.Linq;
using FunctionLib.Steganography;
using ImageToolApp.Models;

namespace ImageToolApp.ViewModels
{
    public class SteganographicModel : BaseViewModel
    {
        private SteganographicAlgorithmImpl mAlgorithm;


        // TODO Shouldnt be here.
        private bool mCompression;

        private int mLsbIndicator = 3;


        public SteganographicModel(SteganographicAlgorithmImpl algorithm)
        {
            Algorithm = algorithm ?? AlgorithmList.FirstOrDefault();
        }

        private Settings Settings
        {
            get { return Settings.Instance; }
        }

        public SteganographicAlgorithmImpl Algorithm
        {
            get { return mAlgorithm; }
            set
            {
                if (value == mAlgorithm)
                {
                    return;
                }
                mAlgorithm = value;
                OnPropertyChanged("Algorithm");
                OnPropertyChanged("ProgressBarValue");
            }
        }

        public IList<SteganographicAlgorithmImpl> AlgorithmList
        {
            get { return Settings.SteganographicMethods; }
        }

        public int LsbIndicator
        {
            get { return mLsbIndicator; }
            set
            {
                if (value.Equals(mLsbIndicator))
                {
                    return;
                }
                mLsbIndicator = value;
                OnPropertyChanged("LsbIndicator");
            }
        }

        public bool Compression
        {
            get { return mCompression; }
            set
            {
                if (value.Equals(mCompression))
                {
                    return;
                }
                mCompression = value;
                OnPropertyChanged("Compression");
            }
        }
    }
}