using UserControlClassLibrary;

namespace ImageToolApp.ViewModels
{
    public class AnalysisTabViewModel : BaseTabViewModel
    {
        private bool mLaplaceGraph;
        private bool mRsAnalysis;
        private bool mSamplePairs;
        private UICommand mSaveToFileCommand;

        public AnalysisTabViewModel()
        {
            RsAnalysis = true;
            SamplePairs = false;
            LaplaceGraph = false;
        }

        public bool RsAnalysis
        {
            get { return mRsAnalysis; }
            set
            {
                if (value == mRsAnalysis)
                {
                    return;
                }
                mRsAnalysis = value;
                OnPropertyChanged();
            }
        }

        public bool SamplePairs
        {
            get { return mSamplePairs; }
            set
            {
                if (value == mSamplePairs)
                {
                    return;
                }
                mSamplePairs = value;
                OnPropertyChanged();
            }
        }

        public bool LaplaceGraph
        {
            get { return mLaplaceGraph; }
            set
            {
                if (value == mLaplaceGraph)
                {
                    return;
                }
                mLaplaceGraph = value;
                OnPropertyChanged();
            }
        }

        public UICommand SaveToFileCommand
        {
            get { return mSaveToFileCommand; }
            set
            {
                if (Equals(value, mSaveToFileCommand))
                {
                    return;
                }
                mSaveToFileCommand = value;
                OnPropertyChanged();
            }
        }
    }
}