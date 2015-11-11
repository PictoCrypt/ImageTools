using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using FunctionLib;
using FunctionLib.Enums;
using UserControlClassLibrary;

namespace ImageToolApp.ViewModels
{
    public class BaseTabViewModel : BaseViewModel
    {
        private bool mEncryptedCheck;
        private string mImagePath = string.Empty;
        private int mNumericUpDownValue = 3;
        private string mPassword;
        private EncryptionMethod mSelectedEncryptionMethod;
        private SteganographicMethod mSelectedSteganographicMethod;
        private UICommand mTabActionCommand;

        public BaseTabViewModel()
        {
            Password = SettingsModel.Password;
            SelectedSteganographicMethod = SettingsModel.SelectedSteganographicMethod;
            SelectedEncryptionMethod = SettingsModel.SelectedEncryptionMethod;
        }

        public int NumericUpDownValue
        {
            get { return mNumericUpDownValue; }
            set
            {
                if (value.Equals(mNumericUpDownValue))
                {
                    return;
                }
                mNumericUpDownValue = value;
                OnPropertyChanged("NumericUpDownValue");
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

        public EncryptionMethod SelectedEncryptionMethod
        {
            get { return mSelectedEncryptionMethod; }
            set
            {
                if (value.Equals(mSelectedEncryptionMethod))
                {
                    return;
                }
                mSelectedEncryptionMethod = value;
                OnPropertyChanged("SelectedEncryptionMethod");
            }
        }

        public ObservableCollection<EncryptionMethod> EncryptionMethods
        {
            get { return SettingsModel.EncryptionMethods; }
        }

        public SteganographicMethod SelectedSteganographicMethod
        {
            get { return mSelectedSteganographicMethod; }
            set
            {
                if (value.Equals(mSelectedSteganographicMethod))
                {
                    return;
                }
                mSelectedSteganographicMethod = value;
                OnPropertyChanged("SelectedSteganographicMethod");
            }
        }

        public ObservableCollection<SteganographicMethod> SteganographicMethods
        {
            get { return SettingsModel.SteganographicMethods; }
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