namespace ImageToolApp.Models
{
    public class DecryptViewModel : BaseViewModel
    {
        private UICommand mChooseImageCommand;
        private string mSourceImagePath;
        private string mText;
        private UICommand mDecryptCommand;

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

        public UICommand DecryptCommand
        {
            get { return mDecryptCommand; }
            set
            {
                if (value.Equals(mDecryptCommand))
                {
                    return;
                }
                mDecryptCommand = value;
                OnPropertyChanged("DecryptCommand");
            }
        }
    }
}