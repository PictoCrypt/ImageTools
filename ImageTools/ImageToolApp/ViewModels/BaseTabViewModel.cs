using ImageToolApp.Models;
using UserControlClassLibrary;

namespace ImageToolApp.ViewModels
{
    public class BaseTabViewModel : BaseViewModel
    {
        private string mImagePath = string.Empty;
        private string mResult;

        private UICommand mTabActionCommand;

        protected BaseTabViewModel()
        {
            Result = string.Empty;
            CryptionModel = new CryptionModel(Settings.Password, Settings.SelectedEncryptionMethod);
            SteganographicModel = new SteganographicModel(Settings.SelectedSteganographicMethod);
        }

        public Settings Settings
        {
            get { return Settings.Instance; }
        }

        public CryptionModel CryptionModel { get; }
        public SteganographicModel SteganographicModel { get; }

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