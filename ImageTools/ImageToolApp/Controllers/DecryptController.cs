using System.IO;
using FunctionLib.Model;
using FunctionLib.Model.Message;
using ImageToolApp.Models;
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
                //TODO Type und Compression
                var model = new DecodeModel(ViewModel.ImagePath, MessageType.Text, ViewModel.SelectedEncryptionMethod,
                    ViewModel.Password, ViewModel.SelectedSteganographicMethod, true, ViewModel.NumericUpDownValue);

                ViewModel.Result = model.Decode();
            });
        }
    }
}