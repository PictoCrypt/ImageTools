using System.Drawing;
using FunctionLib.Steganalyse;
using ImageToolApp.ViewModels;
using ImageToolApp.Views;
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
        }

        private void Benchmark()
        {
            var original = new Bitmap(ViewModel.ImagePath);
            var steganogramm = new Bitmap(ViewModel.Result);
            var benchmarker = new Benchmarker(true, true, true, true, true, true, true, true);
            benchmarker.Run(original, steganogramm);
        }

        private void Anaylsis()
        {
            var steganogramm = new Bitmap(ViewModel.Result);
            var analysis = new StegAnalyser(true, true, true);
            analysis.Run(steganogramm);
        }
    }
}