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
    public class AnalysisTabController : BaseTabController<AnalysisTabViewModel, AnalysisTabView>
    {
        public AnalysisTabController()
        {
            ViewModel.SaveToFileCommand = UICommand.Regular(SaveToFile);
        }

        protected override UICommand ActionCommand
        {
            get { return UICommand.Regular(Analysis); }
        }

        private void SaveToFile()
        {
            var dialog = new SaveFileDialog {Filter = "Text File|*.txt"};
            var dialogResult = dialog.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                using (var stream = File.CreateText(dialog.FileName))
                {
                    stream.Write(ViewModel.Result);
                }
            }
        }

        private void Analysis()
        {
            HandleJobController.Progress(() =>
            {
                var analyser = new StegAnalyser(ViewModel.RsAnalysis, ViewModel.SamplePairs, ViewModel.LaplaceGraph);
                var path = SelectImage();
                if (!string.IsNullOrEmpty(path))
                {
                    using (var bitmap = new Bitmap(path))
                    {
                        ViewModel.Result = analyser.Run(bitmap);
                    }
                }
            });
        }

        private string SelectImage()
        {
            var dialog = new OpenFileDialog
            {
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
    }
}