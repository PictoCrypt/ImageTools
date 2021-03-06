﻿using Microsoft.Win32;

namespace UserControlClassLibrary.PathChooser
{
    public class PathChooserViewModel : PropertyChangedModel
    {
        private UICommand mCommand;
        private string mPath;

        public PathChooserViewModel()
        {
            Command = UICommand.Regular(Open);
        }

        public string Path
        {
            get { return mPath; }
            set
            {
                if (value.Equals(mPath))
                {
                    return;
                }
                mPath = value;
                OnPropertyChanged("Path");
            }
        }

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

        private void Open()
        {
            var dialog = new OpenFileDialog();
            //{ Filter = "Png Image|*.png|Bitmap Image|*.bmp"};
            var dialogResult = dialog.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                Path = dialog.FileName;
            }
        }
    }
}