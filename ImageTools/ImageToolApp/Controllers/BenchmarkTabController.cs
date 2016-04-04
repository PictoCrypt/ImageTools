using System.Drawing;
using System.IO;
using FunctionLib.Helper;
using FunctionLib.Steganalyse;
using ImageToolApp.ViewModels;
using ImageToolApp.Views;
using Microsoft.Win32;
using UserControlClassLibrary;

namespace ImageToolApp.Controllers
{
    public class BenchmarkTabController : BaseTabController<BenchmarkTabViewModel, BenchmarkTabView>
    {
        public BenchmarkTabController()
        {
            ViewModel.SaveToFileCommand = UICommand.Regular(SaveToFile);
        }

        private void SaveToFile()
        {
            var dialog = new SaveFileDialog { Filter = "Text File|*.txt" };
            var dialogResult = dialog.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                using (var stream = File.CreateText(dialog.FileName))
                {
                    stream.Write(ViewModel.Result);
                }
            }
        }

        private void Benchmark()
        {
            HandleJobController.Progress(() =>
            {
                var benchmarker = new Benchmarker(ViewModel.AverageAbsoluteDifference, ViewModel.MeanSquaredError,
                    ViewModel.LpNorm, ViewModel.LaplacianMeanSquaredError,
                    ViewModel.SignalToNoiseRatio, ViewModel.PeakSignalToNoiseRatio, ViewModel.NormalizedCrossCorrelation,
                    ViewModel.CorrelationQuality);
                var original = SelectImage("Select Original Image");
                var stegano = SelectImage("Select Steganogramm");
                if (!string.IsNullOrEmpty(original) || !string.IsNullOrEmpty(stegano))
                {
                    using (var orig = new Bitmap(original))
                    {
                        using (var steg = new Bitmap(stegano))
                        {
                            ViewModel.Result = benchmarker.Run(orig, steg);
                        }
                    }
                }
            });
        }

        private string SelectImage(string title = "")
        {
            var dialog = new OpenFileDialog
            {
                Title = title,
                Filter = ConvertHelper.GenerateFilter(Constants.ImageFormats),
                InitialDirectory = ViewModel.Settings.DefaultPath
            };
            var dialogResult = dialog.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                return dialog.FileName;
            }
            return string.Empty;
        }

        protected override UICommand ActionCommand { get { return UICommand.Regular(Benchmark); } }
    }
}