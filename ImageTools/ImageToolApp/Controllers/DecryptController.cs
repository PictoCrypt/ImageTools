using System.Drawing;
using FunctionLib;
using ImageToolApp.Models;
using ImageToolApp.Views;
using Microsoft.Win32;

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

        public void SaveTxt()
        {
            throw new System.NotImplementedException();
        }

        public override void OpenImage()
        {
            var dialog = new OpenFileDialog();
            dialog.ShowDialog();
            dialog.Multiselect = false;
            if (string.IsNullOrEmpty(dialog.FileName))
            {
                return;
            }
            ViewModel.GlobalViewModel.ImagePath = dialog.FileName;
        }

        private void Decrypt()
        {
            var bitmap = new Bitmap(ViewModel.GlobalViewModel.ImagePath);
            var result = LeastSignificantBit.DecryptText(bitmap);
            if (ViewModel.EncryptedCheck)
            {
                result = Crypto.Decrypt(result, ViewModel.Password);
            }
            ViewModel.Text = result;
        }
    }
}