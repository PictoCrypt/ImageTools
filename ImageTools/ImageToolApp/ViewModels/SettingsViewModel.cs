﻿using System;
using System.Collections.ObjectModel;
using FunctionLib.Enums;
using UserControlClassLibrary;

namespace ImageToolApp.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private UICommand mCancelCommand;
        private UICommand mChoosePathCommand;
        private string mPassword;
        private UICommand mSaveCommand;
        private Type mSelectedEncryptionMethod;
        private SteganographicMethod mSelectedSteganographicMethod;
        private string mStandardPath;

        public SettingsViewModel()
        {
            Password = SettingsModel.Password;
            StandardPath = SettingsModel.StandardPath;
            SelectedEncryptionMethod = SettingsModel.SelectedEncryptionMethod;
            SelectedSteganographicMethod = SettingsModel.SelectedSteganographicMethod;
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
                if (value.Equals(mSelectedEncryptionMethod))
                {
                    return;
                }
                mSelectedEncryptionMethod = value;
            }
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
            }
        }

        public ObservableCollection<Type> EncryptionMethods
        {
            get { return SettingsModel.EncryptionMethods; }
        }

        public ObservableCollection<SteganographicMethod> SteganographicMethods
        {
            get { return SettingsModel.SteganographicMethods; }
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