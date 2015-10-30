using FunctionLib;

namespace ImageToolApp.ViewModels
{
    public class PreferencesViewModel : BaseViewModel
    {
        private UICommand mCancelCommand;
        private string mPassword;
        private UICommand mSaveCommand;
        private EncryptionMethod mSelectedEncryptionMethod;
        private SteganographicMethod mSelectedSteganographicMethod;
        private UICommand mChoosePathCommand;
        private string mStandardPath;

        public PreferencesViewModel()
        {
            Password = PreferencesModel.Password;
            StandardPath = PreferencesModel.StandardPath;
            SelectedEncryptionMethod = PreferencesModel.SelectedEncryptionMethod;
            SelectedSteganographicMethod = PreferencesModel.SelectedSteganographicMethod;
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