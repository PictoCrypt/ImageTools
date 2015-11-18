using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FunctionLib.Cryptography;
using FunctionLib.Steganography;
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
            var dialog = new SaveFileDialog {Filter = "Png Image|*.png|Bitmap Image|*.bmp"};
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
                                bmp.Save(dialog.FileName, ImageFormat.Png);
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
                using (var bitmap = new Bitmap(ViewModel.ImagePath))
                {
                    string value;
                    var currentExpanderContent =
                        Application.Current.Dispatcher.Invoke(
                            () => { return mExpanders.FirstOrDefault(x => x.IsExpanded).Content; });

                    if (currentExpanderContent is PathChooser)
                    {
                        var secretPath =
                            Application.Current.Dispatcher.Invoke(
                                () => ((currentExpanderContent as PathChooser).DataContext as PathChooserViewModel).Path);
                        value = secretPath;
                    }
                    else if (currentExpanderContent is DocumentChooser)
                    {
                        var secretPath =
                            Application.Current.Dispatcher.Invoke(
                                () =>
                                    ((currentExpanderContent as DocumentChooser).DataContext as DocumentChooserViewModel)
                                        .Path);
                        value = secretPath;
                    }
                    else
                    {
                        value = ViewModel.Text;
                        if (ViewModel.EncryptedCheck)
                        {
                            value = SymmetricAlgorithmBase.Encrypt(this, ViewModel.SelectedEncryptionMethod, value,
                                ViewModel.Password);
                        }
                    }
                    var result = SteganographicAlgorithmBase.Encrypt(this, ViewModel.SelectedSteganographicMethod,
                        bitmap, value, ViewModel.NumericUpDownValue);
                    if (result != null)
                    {
                        var path = Path.GetTempFileName().Replace("tmp", "png");
                        result.Save(path);
                        ViewModel.ResultImagePath = path;
                    }
                }
            });
        }

        public void ChangedPixels()
        {
            var path = SteganographicAlgorithmBase.ChangeColor(ViewModel.ResultImagePath, Color.Red);
            var view = new ImagePresentation();
            var count = SteganographicAlgorithmBase.ChangedPixels;
            var viewModel = new ImagePresentationViewModel(path, string.Format("{0} Pixel", count))
            {
                SaveCommand = UICommand.Regular(() =>
                {
                    var dialog = new SaveFileDialog
                    {
                        Filter = "Png Image|*.png|Bitmap Image|*.bmp",
                        InitialDirectory = ViewModel.SettingsModel.StandardPath
                    };
                    var dialogResult = dialog.ShowDialog();
                    if (dialogResult.HasValue && dialogResult.Value)
                    {
                        var tmp = path;
                        using (var bmp = new Bitmap(tmp))
                        {
                            if (File.Exists(dialog.FileName))
                            {
                                File.Delete(dialog.FileName);
                            }

                            switch (dialog.FilterIndex)
                            {
                                case 0:
                                    bmp.Save(dialog.FileName, ImageFormat.Png);
                                    break;
                                case 1:
                                    bmp.Save(dialog.FileName, ImageFormat.Bmp);
                                    break;
                            }
                        }
                    }
                })
            };
            view.DataContext = viewModel;
            view.Show();
        }
    }
}