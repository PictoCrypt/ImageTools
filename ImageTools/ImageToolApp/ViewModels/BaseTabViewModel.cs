using System;
using System.Collections.Generic;
using System.Linq;
using ImageToolApp.Models;
using UserControlClassLibrary;

namespace ImageToolApp.ViewModels
{
    public class BaseTabViewModel : BaseViewModel
    {
        private bool mEncryptedCheck;
        private string mImagePath = string.Empty;
        private int mNumericUpDownValue = 3;
        private string mPassword;
        private Type mSelectedEncryptionMethod;
        private Type mSelectedSteganographicMethod;
        private UICommand mTabActionCommand;

        public BaseTabViewModel()
        {
            Password = Settings.Password;
            SelectedSteganographicMethod = Settings.SelectedSteganographicMethod ??
                                           Settings.SteganographicMethods.FirstOrDefault();
            SelectedEncryptionMethod = Settings.SelectedEncryptionMethod ??
                                       Settings.EncryptionMethods.FirstOrDefault().Value;
        }

        public Settings Settings
        {
            get { return Settings.Instance; }
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

        public Type SelectedEncryptionMethod
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

        public IDictionary<string, Type> EncryptionMethods
        {
            get { return Settings.EncryptionMethods; }
        }

        public Type SelectedSteganographicMethod
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

        public IList<Type> SteganographicMethods
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