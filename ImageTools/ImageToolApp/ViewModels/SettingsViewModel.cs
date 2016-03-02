using System;
using System.Collections.Generic;
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
        private Type mSelectedEncryptionMethod;
        private Type mSelectedSteganographicMethod;
        private string mStandardPath;

        public SettingsViewModel(Settings settings)
        {
            mSettings = settings;
            Password = mSettings.Password;
            StandardPath = mSettings.StandardPath;
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
            }
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
            }
        }

        public IDictionary<string, Type> EncryptionMethods
        {
            get { return mSettings.EncryptionMethods; }
        }

        public IList<Type> SteganographicMethods
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