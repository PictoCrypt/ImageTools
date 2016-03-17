using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FunctionLib.Helper;
using FunctionLib.Model;
using ImageToolApp.ViewModels;
using ImageToolApp.Views;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using UserControlClassLibrary;
using UserControlClassLibrary.DocumentChooser;
using UserControlClassLibrary.PathChooser;
using Image = System.Drawing.Image;

namespace ImageToolApp.Controllers
{
    public class EncryptController : BaseTabController<EncryptTabViewModel, EncryptTabView>
    {
        private readonly List<Expander> mExpanders;

        private bool mExpanderAlreadyHandled;

        public EncryptController()
        {
            mExpanders =
                View.FindChildren<Expander>()
                    .Where(x => x.Content != null && x.Content.GetType() != typeof (Image))
                    .ToList();
            foreach (var expander in mExpanders)
            {
                expander.Expanded += ExpanderOnExpanded;
                expander.Collapsed += ExpanderOnCollapsed;
            }
        }

        public override void UnregisterEvents()
        {
            base.UnregisterEvents();
            foreach (var expander in mExpanders)
            {
                expander.Expanded -= ExpanderOnExpanded;
                expander.Collapsed -= ExpanderOnCollapsed;
            }
        }

        private void ExpanderOnExpanded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (!mExpanderAlreadyHandled)
            {
                var expander = sender as Expander;
                mExpanderAlreadyHandled = true;
                foreach (var exp in mExpanders)
                {
                    exp.IsExpanded = false;
                    var row = View.Grid.RowDefinitions[Grid.GetRow(exp)];
                    row.Height = GridLength.Auto;
                }

                expander.IsExpanded = true;
                View.Grid.RowDefinitions[Grid.GetRow(expander)].Height = new GridLength(1, GridUnitType.Star);
                routedEventArgs.Handled = true;
                mExpanderAlreadyHandled = false;
            }
        }

        private void ExpanderOnCollapsed(object sender, RoutedEventArgs routedEventArgs)
        {
            foreach (var exp in mExpanders)
            {
                exp.IsExpanded = false;
                var row = View.Grid.RowDefinitions[Grid.GetRow(exp)];
                row.Height = GridLength.Auto;
            }
            routedEventArgs.Handled = true;
        }

        protected override void RegisterCommands()
        {
            ViewModel.TabActionCommand = UICommand.Regular(Encrypt);
        }

        public void SaveImage()
        {
            var dialog = new SaveFileDialog {Filter = "PNG Image|*.png|Bitmap Image|*.bmp"};
            var dialogResult = dialog.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                var tmp = ViewModel.ResultImagePath;
                if (File.Exists(dialog.FileName))
                {
                    File.Delete(dialog.FileName);
                }
                ViewModel.ResultImagePath = dialog.FileName;


                switch (dialog.FilterIndex)
                {
                    case 0:
                        HandleJobController.Progress(() =>
                        {
                            using (var bmp = new Bitmap(tmp))
                            {
                                bmp.Save(dialog.FileName, ImageFormat.Png);
                            }
                        });
                        break;
                    case 1:
                        HandleJobController.Progress(() =>
                        {
                            using (var bmp = new Bitmap(tmp))
                            {
                                bmp.Save(dialog.FileName, ImageFormat.Bmp);
                            }
                        });
                        break;
                }
            }
        }

        public void OpenTxt()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Text File|*.txt",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };
            var dialogResult = dialog.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                ViewModel.Text = File.ReadAllText(dialog.FileName);
            }
        }

        private void Encrypt()
        {
            HandleJobController.Progress(() =>
            {
                try
                {
                    var message = GetCurrentMessage();
                    //TODO compression
                    var model = new EncodeModel(ViewModel.ImagePath, message, ViewModel.SelectedEncryptionMethod,
                        ViewModel.Password, ViewModel.SelectedSteganographicMethod, false,
                        ViewModel.LsbIndicator);

                    var result = model.Encode();
                    if (result != null)
                    {
                        var path = FileManager.GetInstance().GenerateTmp(ImageFormat.Png);
                        result.Save(path);
                        ViewModel.ResultImagePath = path;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.InnerException.Message,
                        "Fehler",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            });
        }

        private string GetCurrentMessage()
        {
            string result;
            object expanderContent = null;
            if (mExpanders != null && mExpanders.Count > 0)
            {
                expanderContent = Application.Current.Dispatcher.Invoke(() =>
                {
                    var current = mExpanders.FirstOrDefault(x => x.IsExpanded);
                    return current != null ? current.Content : null;
                });
            }

            if (expanderContent == null)
            {
                result = ViewModel.Text;
            }
            else
            {
                if (expanderContent is TextBox)
                {
                    result = ViewModel.Text;
                }
                else if (expanderContent is PathChooser)
                {
                    var content = expanderContent as PathChooser;
                    result = (content.DataContext as PathChooserViewModel).Path;
                }
                else
                {
                    var content = expanderContent as DocumentChooser;
                    result = (content.DataContext as DocumentChooserViewModel).Path;
                }
            }
            return result;
        }

        public void ChangedPixels()
        {
            var path = ViewModel.SelectedSteganographicMethod.ChangeColor(ViewModel.ResultImagePath, Color.Red);
            var count = ViewModel.SelectedSteganographicMethod.ChangedPixels.Count;
            var controller = new ImagePresentationController(path, string.Format("{0} Pixel", count));
        }
    }
}