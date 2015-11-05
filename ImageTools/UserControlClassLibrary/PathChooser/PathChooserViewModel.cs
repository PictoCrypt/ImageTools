using Microsoft.Win32;

namespace UserControlClassLibrary.PathChooser
{
    public class PathChooserViewModel : PropertyChangedModel
    {
        public PathChooserViewModel()
        {
            Command = UICommand.Regular(Open);
        }

        private void Open()
        {
            var dialog = new OpenFileDialog { Filter = "Png Image|*.png|Bitmap Image|*.bmp" };
            var dialogResult = dialog.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                Path = dialog.FileName;
            }
        }

        public string Path
        {
            get { return mPath; }
            set
            {
                if(value.Equals(mPath))
                {
                    return;
                }
                mPath = value;
                OnPropertyChanged("Path");
            }
        }

        private UICommand mCommand;
        private string mPath;

        public UICommand Command
        {
            get { return mCommand; }
            set
            {
                if (value.Equals(mCommand))
                {
                    return;
                }
                mCommand = value;
                OnPropertyChanged("Command");
            }
        }
    }
}