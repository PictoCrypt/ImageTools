using System.Drawing;
using System.IO;
using FunctionLib.Cryptography;
using FunctionLib.Steganography;
using ImageToolApp.ViewModels;
using Microsoft.Win32;
using UserControlClassLibrary;

namespace ImageToolApp.Controllers
{
    public class DecryptController : BaseTabController<BaseTabViewModel>
    {
        public DecryptController(MainController mainController, string viewName, bool textBoxReadOnly) : base(mainController, viewName, textBoxReadOnly)
        {
        }

        protected override void RegisterCommands()
        {
            ViewModel.TabActionCommand = UICommand.Regular(Decrypt);
        }

        public void SaveTxt()
        {
            var dialog = new SaveFileDialog {Filter = "Text File|*.txt"};
            var dialogResult = dialog.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                using (var stream = File.CreateText(dialog.FileName))
                {
                    stream.Write(ViewModel.Text);
                }
            }
        }

        private void Decrypt()
        {
            using (var handler = new HandleJobController())
            {
                handler.Progress(() =>
                {
                    string result;
                    using (var bitmap = new Bitmap(ViewModel.ImagePath))
                    {
                        result = SteganographicAlgorithmBase.Decrypt(this, ViewModel.SelectedSteganographicMethod, bitmap, ViewModel.NumericUpDownValue);
                    }

                    if (ViewModel.EncryptedCheck)
                    {
                        result = SymmetricAlgorithmBase.Decrypt(this, ViewModel.SelectedEncryptionMethod, result,
                            ViewModel.Password);
                    }

                    ViewModel.Text = result;                    
                });
            }
        }
    }
}