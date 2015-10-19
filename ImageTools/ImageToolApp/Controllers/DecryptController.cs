using System;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using ImageFunctionLib;
using ImageToolApp.Models;
using ImageToolApp.Views;

namespace ImageToolApp.Controllers
{
    public class DecryptController : BaseTabController<DecryptView, DecryptViewModel>
    {
        protected override DecryptView CreateView()
        {
            return new DecryptView();
        }

        protected override void RegisterCommands()
        {
            base.RegisterCommands();
            ViewModel.DecryptCommand = UICommand.Regular(Decrypt);
        }

        private void Decrypt()
        {
            var bitmap = new Bitmap(ViewModel.SourceImagePath);
            var result = LeastSignificantBit.DecryptText(bitmap);
            ViewModel.Text = result;
        }
    }
}