using System.Collections.Generic;
using FunctionLib.Cryptography;
using FunctionLib.Steganography.Base;
using ImageToolApp.Models;
using UserControlClassLibrary;

namespace ImageToolApp.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly Settings mSettings;
        private UICommand mCancelCommand;
        private UICommand mChoosePathCommand;
        private string mPassword;
        private UICommand mSaveCommand;
        private CryptographicAlgorithmImpl mSelectedEncryptionMethod;
        private SteganographicAlgorithmImpl mSelectedSteganographicMethod;
        private string mStandardPath;

        public SettingsViewModel(Settings settings)
        {
            mSettings = settings;
            Password = mSettings.Password;
            StandardPath = mSettings.DefaultPath;
            SelectedEncryptionMethod = mSettings.SelectedEncryptionMethod;
            SelectedSteganographicMethod = mSettings.SelectedSteganographicMethod;
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
            }
        }

        public string StandardPath
        {
            get { return mStandardPath; }
            set
            {
                if (value == mStandardPath) return;
                mStandardPath = value;
                OnPropertyChanged("StandardPath");
            }
        }

        public CryptographicAlgorithmImpl SelectedEncryptionMethod
        {
            get { return mSelectedEncryptionMethod; }
            set
            {
                if (value.Equals(mSelectedEncryptionMethod))
                {
                    return;
                }
                mSelectedEncryptionMethod = value;
            }
        }

        public SteganographicAlgorithmImpl SelectedSteganographicMethod
        {
            get { return mSelectedSteganographicMethod; }
            set
            {
                if (value.Equals(mSelectedSteganographicMethod))
                {
                    return;
                }
                mSelectedSteganographicMethod = value;
            }
        }

        public IList<CryptographicAlgorithmImpl> EncryptionMethods
        {
            get { return mSettings.EncryptionMethods; }
        }

        public IList<SteganographicAlgorithmImpl> SteganographicMethods
        {
            get { return mSettings.SteganographicMethods; }
        }

        public UICommand SaveCommand
        {
            get { return mSaveCommand; }
            set
            {
                if (value.Equals(mSaveCommand))
                {
                    return;
                }
                mSaveCommand = value;
                OnPropertyChanged("SaveCommand");
            }
        }

        public UICommand CancelCommand
        {
            get { return mCancelCommand; }
            set
            {
                if (value.Equals(mCancelCommand))
                {
                    return;
                }
                mCancelCommand = value;
                OnPropertyChanged("CancelCommand");
            }
        }

        public UICommand ChoosePathCommand
        {
            get { return mChoosePathCommand; }
            set
            {
                if (Equals(value, mChoosePathCommand)) return;
                mChoosePathCommand = value;
                OnPropertyChanged("ChoosePathCommand");
            }
        }
    }
}