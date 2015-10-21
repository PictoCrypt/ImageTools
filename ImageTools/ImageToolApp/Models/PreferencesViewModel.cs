using System;
using System.Collections.ObjectModel;
using System.Linq;
using FunctionLib;

namespace ImageToolApp.Models
{
    public class PreferencesViewModel : BaseViewModel
    {
        private UICommand mCancelCommand;
        private string mPassword;
        private UICommand mSaveCommand;
        private EncryptionMethod mSelectedEncryptionMethod;
        private SteganographicMethod mSelectedSteganographicMethod;

        public PreferencesViewModel(GlobalViewModel model)
        {
            Model = model;
            EncryptionMethods = new ObservableCollection<EncryptionMethod>(Enum.GetValues(typeof (EncryptionMethod))
                .Cast<EncryptionMethod>());
            SteganographicMethods =
                new ObservableCollection<SteganographicMethod>(Enum.GetValues(typeof (SteganographicMethod))
                    .Cast<SteganographicMethod>());
        }

        public GlobalViewModel Model { get; private set; }

        public ObservableCollection<EncryptionMethod> EncryptionMethods { get; set; }
        public ObservableCollection<SteganographicMethod> SteganographicMethods { get; set; }

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
    }
}