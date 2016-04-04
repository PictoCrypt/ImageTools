using UserControlClassLibrary;

namespace ImageToolApp.ViewModels
{
    public class BenchmarkTabViewModel : BaseTabViewModel
    {
        public BenchmarkTabViewModel()
        {
            AverageAbsoluteDifference = true;
            CorrelationQuality = true;
            LpNorm = true;
            NormalizedCrossCorrelation = true;
            MeanSquaredError = true;
            LaplacianMeanSquaredError = true;
            SignalToNoiseRatio = true;
            PeakSignalToNoiseRatio = true;
        }

        public bool AverageAbsoluteDifference
        {
            get { return mAverageAbsoluteDifference; }
            set
            {
                if (value == mAverageAbsoluteDifference)
                {
                    return;
                }
                mAverageAbsoluteDifference = value;
                OnPropertyChanged();
            }
        }

        public bool CorrelationQuality
        {
            get { return mCorrelationQuality; }
            set
            {
                if (value == mCorrelationQuality)
                {
                    return;
                }
                mCorrelationQuality = value;
                OnPropertyChanged();
            }
        }

        public bool LpNorm
        {
            get { return mLpNorm; }
            set
            {
                if (value == mLpNorm)
                {
                    return;
                }
                mLpNorm = value;
                OnPropertyChanged();
            }
        }

        public bool NormalizedCrossCorrelation
        {
            get { return mNormalizedCrossCorrelation; }
            set
            {
                if (value == mNormalizedCrossCorrelation)
                {
                    return;
                }
                mNormalizedCrossCorrelation = value;
                OnPropertyChanged();
            }
        }

        public bool MeanSquaredError
        {
            get { return mMeanSquaredError; }
            set
            {
                if (value == mMeanSquaredError)
                {
                    return;
                }
                mMeanSquaredError = value;
                OnPropertyChanged();
            }
        }

        public bool LaplacianMeanSquaredError
        {
            get { return mLaplacianMeanSquaredError; }
            set
            {
                if (value == mLaplacianMeanSquaredError)
                {
                    return;
                }
                mLaplacianMeanSquaredError = value;
                OnPropertyChanged();
            }
        }

        public bool SignalToNoiseRatio
        {
            get { return mSignalToNoiseRatio; }
            set
            {
                if (value == mSignalToNoiseRatio)
                {
                    return;
                }
                mSignalToNoiseRatio = value;
                OnPropertyChanged();
            }
        }

        public bool PeakSignalToNoiseRatio
        {
            get { return mPeakSignalToNoiseRatio; }
            set
            {
                if (value == mPeakSignalToNoiseRatio)
                {
                    return;
                }
                mPeakSignalToNoiseRatio = value;
                OnPropertyChanged();
            }
        }


        private UICommand mSaveToFileCommand;
        private bool mAverageAbsoluteDifference;
        private bool mCorrelationQuality;
        private bool mLpNorm;
        private bool mNormalizedCrossCorrelation;
        private bool mMeanSquaredError;
        private bool mLaplacianMeanSquaredError;
        private bool mSignalToNoiseRatio;
        private bool mPeakSignalToNoiseRatio;

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