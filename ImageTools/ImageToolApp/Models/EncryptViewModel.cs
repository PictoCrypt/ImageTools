namespace ImageToolApp.Models
{
    public class EncryptViewModel : BaseTabViewModel
    {
        private UICommand mSaveImageCommand;
        private UICommand mEncryptCommand;
        private string mResultImagePath;
        private UICommand mEncryptWithAesCommand;

        public UICommand SaveImageCommand
        {
            get { return mSaveImageCommand; }
            set
            {
                if (value.Equals(mSaveImageCommand))
                {
                    return;
                }
                mSaveImageCommand = value;
                OnPropertyChanged("SaveImageCommand");
            }
        }

        public UICommand EncryptWithAesCommand
        {
            get { return mEncryptWithAesCommand; }
            set
            {
                if (value.Equals(mEncryptWithAesCommand))
                {
                    return;
                }
                mEncryptWithAesCommand = value;
                OnPropertyChanged("EncryptWithAesCommand");
            }
        }

        public UICommand EncryptCommand
        {
            get { return mEncryptCommand; }
            set
            {
                if (value.Equals(mEncryptCommand))
                {
                    return;
                }
                mEncryptCommand = value;
                OnPropertyChanged("EncryptCommand");
            }
        }

        public string ResultImagePath
        {
            get { return mResultImagePath; }
            set
            {
                if (value.Equals(mResultImagePath))
                {
                    return;
                }
                mResultImagePath = value;
                OnPropertyChanged("ResultImagePath");
            }
        }
    }
}