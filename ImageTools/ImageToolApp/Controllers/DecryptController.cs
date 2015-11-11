using System.Drawing;
using System.IO;
using FunctionLib;
using FunctionLib.Cryptography;
using FunctionLib.Enums;
using FunctionLib.Steganography;
using ImageToolApp.ViewModels;
using ImageToolApp.Views;
using Microsoft.Win32;
using UserControlClassLibrary;

namespace ImageToolApp.Controllers
{
    public class DecryptController : BaseTabController<DecryptTabViewModel, DecryptTabView>
    {
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
                    stream.Write(ViewModel.Result);
                }
            }
        }

        private void Decrypt()
        {
            HandleJobController.Progress(() =>
            {
                object result;
                using (var bitmap = new Bitmap(ViewModel.ImagePath))
                {
                    result = SteganographicAlgorithmBase.Decrypt(this, ViewModel.SelectedSteganographicMethod, bitmap,
                        ViewModel.SelectedResultingType, ViewModel.NumericUpDownValue);
                }

                if (ViewModel.EncryptedCheck && ViewModel.SelectedResultingType == ResultingType.Text)
                {
                    result = SymmetricAlgorithmBase.Decrypt(this, ViewModel.SelectedEncryptionMethod, result.ToString(),
                        ViewModel.Password);
                }
                ViewModel.Result = result;
            });
        }
    }
}