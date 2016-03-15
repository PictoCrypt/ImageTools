using System.Collections.Generic;
using System.Linq;
using FunctionLib.Cryptography;
using FunctionLib.Steganography.Base;
using ImageToolApp.Models;
using UserControlClassLibrary;

namespace ImageToolApp.ViewModels
{
    public class BaseTabViewModel : BaseViewModel
    {
        private bool mEncryptedCheck;
        private string mImagePath = string.Empty;
        private int mLsbIndicator = 3;
        private string mPassword;
        private CryptographicAlgorithmImpl mSelectedEncryptionMethod;
        private SteganographicAlgorithmImpl mSelectedSteganographicMethod;
        private UICommand mTabActionCommand;

        public BaseTabViewModel()
        {
            Password = Settings.Password;
            SelectedSteganographicMethod = Settings.SelectedSteganographicMethod ??
                                           Settings.SteganographicMethods.FirstOrDefault();
            SelectedEncryptionMethod = Settings.SelectedEncryptionMethod ??
                                       Settings.EncryptionMethods.FirstOrDefault();
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

        public CryptographicAlgorithmImpl SelectedEncryptionMethod
        {
            get { return mSelectedEncryptionMethod; }
            set
            {
                if (value == mSelectedEncryptionMethod)
                {
                    return;
                }
                mSelectedEncryptionMethod = value;
                OnPropertyChanged("SelectedEncryptionMethod");
            }
        }

        public IList<CryptographicAlgorithmImpl> EncryptionMethods
        {
            get { return Settings.EncryptionMethods; }
        }

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
            }
        }

        public IList<SteganographicAlgorithmImpl> SteganographicMethods
        {
            get { return Settings.SteganographicMethods; }
        }

        public string Password
        {
            get { return mPassword; }
            set
            {
                if (value.Equals(mPassword))
                {
                    return;
                }
                mPassword = value;
                OnPropertyChanged("Password");
                if (!string.IsNullOrEmpty(mPassword))
                {
                    EncryptedCheck = true;
                }
            }
        }

        public bool EncryptedCheck
        {
            get { return mEncryptedCheck || !string.IsNullOrEmpty(Password); }
            set
            {
                if (value.Equals(mEncryptedCheck))
                {
                    return;
                }
                mEncryptedCheck = value;
                OnPropertyChanged("EncryptedCheck");
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
            }
        }
    }
}