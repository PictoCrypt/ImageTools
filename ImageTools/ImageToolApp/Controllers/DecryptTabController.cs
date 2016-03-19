using System.IO;
using FunctionLib.Model;
using ImageToolApp.ViewModels;
using ImageToolApp.Views;
using Microsoft.Win32;
using UserControlClassLibrary;

namespace ImageToolApp.Controllers
{
    public class DecryptTabController : BaseTabController<DecryptTabViewModel, DecryptTabView>
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
                //TODO Compression
                var model = new DecodeModel(ViewModel.ImagePath, ViewModel.SelectedEncryptionMethod,
                    ViewModel.Password, ViewModel.SelectedSteganographicMethod, false, ViewModel.LsbIndicator);

                ViewModel.Result = model.Decode();
            });
        }
    }
}