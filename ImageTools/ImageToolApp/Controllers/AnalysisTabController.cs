using System.Drawing;
using FunctionLib.Helper;
using FunctionLib.Steganalyse;
using ImageToolApp.ViewModels;
using ImageToolApp.Views;
using Microsoft.Win32;
using UserControlClassLibrary;

namespace ImageToolApp.Controllers
{
    public class AnalysisTabController : BaseTabController<AnalysisTabViewModel, AnalysisTabView>
    {
        public AnalysisTabController()
        {
            RegisterCommands();
        }

        protected override UICommand ActionCommand { get { return UICommand.Regular(DoNothing); } }

        private void DoNothing()
        {
            throw new System.NotImplementedException();
        }


        private void RegisterCommands()
        {
            ViewModel.AnalysisCommand = UICommand.Regular(Anaylsis);
            ViewModel.BenchmarkCommand = UICommand.Regular(Benchmark);
            ViewModel.LoadImageCommand = UICommand.Regular(LoadImage);
            ViewModel.LoadSteganoCommand = UICommand.Regular(LoadStegano);
        }

        private void LoadStegano()
        {
            var path = Load();
            ViewModel.Result = path;
        }

        private void LoadImage()
        {
            var path = Load();
            ViewModel.ImagePath = path;
        }

        private string Load()
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = false,
                InitialDirectory = ViewModel.Settings.DefaultPath,
                Filter = ConvertHelper.GenerateFilter(Constants.ImageFormats)
            };
            var result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                return dialog.FileName;
            }
            return string.Empty;
        }

        private void Benchmark()
        {
            var original = new Bitmap(ViewModel.ImagePath);
            var steganogramm = new Bitmap(ViewModel.Result);
            var benchmarker = new Benchmarker(true, true, true, true, true, true, true, true);
            var result = benchmarker.Run(original, steganogramm);
        }

        private void Anaylsis()
        {
            var steganogramm = new Bitmap(ViewModel.Result);
            var analysis = new StegAnalyser(true, true, true);
            var result = analysis.Run(steganogramm);
        }
    }
}