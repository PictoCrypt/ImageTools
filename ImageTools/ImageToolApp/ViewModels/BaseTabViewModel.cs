using System.Collections.Generic;
using System.Linq;
using FunctionLib.Steganography;
using ImageToolApp.Models;
using UserControlClassLibrary;

namespace ImageToolApp.ViewModels
{
    public class BaseTabViewModel : BaseViewModel
    {
        private string mImagePath = string.Empty;
        private int mLsbIndicator = 3;
        private SteganographicAlgorithmImpl mSelectedSteganographicMethod;
        private UICommand mTabActionCommand;
        private bool mCompression;
        private string mResult;
        private readonly EncryptionModel mEncryptionModel;

        protected BaseTabViewModel()
        {
            Result = string.Empty;
            mEncryptionModel = new EncryptionModel(Settings.Password, Settings.SelectedEncryptionMethod);
            SelectedSteganographicMethod = Settings.SelectedSteganographicMethod ??
                                           Settings.SteganographicMethods.FirstOrDefault();
        }

        public Settings Settings
        {
            get { return Settings.Instance; }
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

        public EncryptionModel EncryptionModel { get { return mEncryptionModel; } }

        public SteganographicAlgorithmImpl SelectedSteganographicMethod
        {
            get { return mSelectedSteganographicMethod; }
            set
            {
                if (value == mSelectedSteganographicMethod)
                {
                    return;
                }
                mSelectedSteganographicMethod = value;
                OnPropertyChanged("SelectedSteganographicMethod");
                OnPropertyChanged("ProgressBarValue");
            }
        }

        public IList<SteganographicAlgorithmImpl> SteganographicMethods
        {
            get { return Settings.SteganographicMethods; }
        }


        public virtual bool CanTabActionExecuted
        {
            get { return !string.IsNullOrEmpty(ImagePath); }
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

        public string ImagePath
        {
            get { return mImagePath; }
            set
            {
                if (value.Equals(mImagePath))
                {
                    return;
                }
                mImagePath = value;
                OnPropertyChanged("ImagePath");
                OnPropertyChanged("ProgressBarValue");
                OnPropertyChanged("CanTabActionExecuted");
            }
        }

        public string Result
        {
            get { return mResult; }
            set
            {
                if (value.Equals(mResult))
                {
                    return;
                }
                mResult = value;
                OnPropertyChanged("Result");
            }
        }
    }
}