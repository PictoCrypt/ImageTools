using System.Collections.Generic;
using System.Linq;
using FunctionLib.Steganography;
using ImageToolApp.Models;
using UserControlClassLibrary;

namespace ImageToolApp.ViewModels
{
    public class SteganographicModel : BaseViewModel
    {
        private Settings Settings { get { return Settings.Instance; } }


        public SteganographicModel(SteganographicAlgorithmImpl algorithm)
        { 
            Algorithm = algorithm ?? AlgorithmList.FirstOrDefault();
        }

        private int mLsbIndicator = 3;
        private SteganographicAlgorithmImpl mAlgorithm;
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

        private UICommand mTabActionCommand;
        public UICommand TabActionCommand
        {
            get { return mTabActionCommand; }
            set
            {
                if (value.Equals(mTabActionCommand))
                {
                    return;
                }
                mTabActionCommand = value;
                OnPropertyChanged("TabActionCommand");
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


        // TODO Shouldnt be here.
        private bool mCompression;
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