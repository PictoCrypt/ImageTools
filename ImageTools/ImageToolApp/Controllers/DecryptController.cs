using System.Drawing;
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
            ViewModel.DecryptTextCommand = UICommand.Regular(DecryptText);
        }

        private void DecryptText()
        {
            var result = Crypto.Decrypt(ViewModel.Text, ViewModel.AesPassword);
            ViewModel.Text = result;
        }

        private void Decrypt()
        {
            var bitmap = new Bitmap(ViewModel.SourceImagePath);
            var result = LeastSignificantBit.DecryptText(bitmap);
            ViewModel.Text = result;
        }
    }
}