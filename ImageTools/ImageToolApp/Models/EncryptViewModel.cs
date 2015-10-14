namespace ImageToolApp.Models
{
    public class EncryptViewModel : BaseViewModel
    {
        private UICommand mChooseImageCommand;
        private UICommand mSaveImageCommand;
        private string mText;
        private UICommand mEncryptCommand;
        private string mSourceImagePath;
        private string mResultImagePath;

        public UICommand ChooseImageCommand
        {
            get { return mChooseImageCommand; }
            set
            {
                if (value.Equals(mChooseImageCommand))
                {
                    return;
                }
                mChooseImageCommand = value;
                OnPropertyChanged("ChooseImageCommand");
            }
        }

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

        public string Text
        {
            get { return mText; }
            set
            {
                if (value.Equals(mText))
                {
                    return;
                }
                mText = value;
                OnPropertyChanged("Text");
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

        public string SourceImagePath
        {
            get { return mSourceImagePath; }
            set
            {
                if (value.Equals(mSourceImagePath))
                {
                    return;
                }
                mSourceImagePath = value;
                OnPropertyChanged("SourceImagePath");
            }
        }
    }
}