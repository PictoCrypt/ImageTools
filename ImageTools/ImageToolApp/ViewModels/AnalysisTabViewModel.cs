using UserControlClassLibrary;

namespace ImageToolApp.ViewModels
{
    public class AnalysisTabViewModel : BaseTabViewModel
    {
        private UICommand mBenchmarkCommand;
        private UICommand mAnalysisCommand;

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