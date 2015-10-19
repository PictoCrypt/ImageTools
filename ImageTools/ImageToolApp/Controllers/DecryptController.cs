using System.Drawing;
using FunctionLib;
using FunctionLib.Cryptography;
using FunctionLib.Steganography;
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

        private void Decrypt()
        {
            string result;
            using (var bitmap = new Bitmap(ViewModel.GlobalViewModel.ImagePath))
            {
                result = LeastSignificantBit.DecryptText(bitmap);
            }

            if (ViewModel.EncryptedCheck)
            {
                result = Crypto.Decrypt(result, ViewModel.Password);
            }
            ViewModel.Text = result;
        }
    }
}