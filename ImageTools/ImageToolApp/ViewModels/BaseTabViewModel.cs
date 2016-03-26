using ImageToolApp.Models;
using UserControlClassLibrary;

namespace ImageToolApp.ViewModels
{
    public class BaseTabViewModel : BaseViewModel
    {
        private string mImagePath = string.Empty;
        private string mResult;
        private readonly CryptionModel mCryptionModel;
        private readonly SteganographicModel mSteganographicModel;

        protected BaseTabViewModel()
        {
            Result = string.Empty;
            mCryptionModel = new CryptionModel(Settings.Password, Settings.SelectedEncryptionMethod);
            mSteganographicModel = new SteganographicModel(Settings.SelectedSteganographicMethod);
        }

        public Settings Settings
        {
            get { return Settings.Instance; }
        }

        public CryptionModel CryptionModel { get { return mCryptionModel; } }
        public SteganographicModel SteganographicModel { get { return mSteganographicModel; } }

        public virtual bool CanTabActionExecuted
        {
            get { return !string.IsNullOrEmpty(ImagePath); }
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