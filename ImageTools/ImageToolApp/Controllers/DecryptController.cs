using System.Drawing;
using FunctionLib;
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
            ViewModel.TabActionCommand = UICommand.Regular(Decrypt);
        }

        private void Decrypt()
        {
            var bitmap = new Bitmap(ViewModel.GlobalViewModel.ImagePath);
            var result = LeastSignificantBit.DecryptText(bitmap);
            result = Crypto.Decrypt(result, ViewModel.Password);
            ViewModel.Text = result;
        }
    }
}