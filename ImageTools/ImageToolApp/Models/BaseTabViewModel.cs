namespace ImageToolApp.Models
{
    public class BaseTabViewModel : BaseViewModel
    {
        private UICommand mChooseImageCommand;
        private string mSourceImagePath;
        private string mText;
        private string mAesPassword;

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

        public string AesPassword
        {
            get { return mAesPassword; }
            set
            {
                if (value.Equals(mAesPassword))
                {
                    return;
                }
                mAesPassword = value;
                OnPropertyChanged("AesPassword");
            }
        }
    }
}