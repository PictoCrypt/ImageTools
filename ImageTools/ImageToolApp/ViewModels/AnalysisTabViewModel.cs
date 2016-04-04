using UserControlClassLibrary;

namespace ImageToolApp.ViewModels
{
    public class AnalysisTabViewModel : BaseTabViewModel
    {
        private UICommand mBenchmarkCommand;
        private UICommand mAnalysisCommand;
        private UICommand mLoadImageCommand;
        private UICommand mLoadSteganoCommand;

        public UICommand LoadImageCommand
        {
            get { return mLoadImageCommand; }
            set
            {
                if (value.Equals(mLoadImageCommand))
                {
                    return;
                }
                mLoadImageCommand = value;
                OnPropertyChanged("LoadImageCommand");
            }
        }

        public UICommand LoadSteganoCommand
        {
            get { return mLoadSteganoCommand; }
            set
            {
                if (value.Equals(mLoadSteganoCommand))
                {
                    return;
                }
                mLoadSteganoCommand = value;
                OnPropertyChanged("LoadSteganoCommand");
            }
        }

        public UICommand BenchmarkCommand
        {
            get { return mBenchmarkCommand; }
            set
            {
                if (value.Equals(mBenchmarkCommand))
                {
                    return;
                }
                mBenchmarkCommand = value;
                OnPropertyChanged("BenchmarkCommand");
            }
        }

        public UICommand AnalysisCommand
        {
            get { return mAnalysisCommand; }
            set
            {
                if (value.Equals(mAnalysisCommand))
                {
                    return;
                }
                mAnalysisCommand = value;
                OnPropertyChanged("AnalysisCommand");
            }
        }
    }
}