using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ImageToolApp.ViewModels;
using ImageToolApp.Views;
using MahApps.Metro.Controls;
using Microsoft.Win32;

namespace ImageToolApp.Controllers
{
    public abstract class BaseTabController<TViewModel> : IBaseTabController where TViewModel : BaseTabViewModel, new()
    {
        protected readonly List<Expander> Expanders;
        // TODO: Tmp-Files löschen nach gebrauch?

        protected readonly TViewModel ViewModel;
        protected MainController MainController;

        protected BaseTabController(MainController mainController, string viewName, bool textBoxReadOnly,
            bool resultingTypeVisibile)
        {
            MainController = mainController;
            View = CreateView(viewName, textBoxReadOnly, resultingTypeVisibile);
            ViewModel = new TViewModel();
            View.DataContext = ViewModel;
            InitializeController();
            View.ImageExpander.Expanded += ImageExpanderEvent;
            View.ImageExpander.Collapsed += ImageExpanderEvent;

            Expanders =
                View.FindChildren<Expander>()
                    .Where(x => x.Content != null && x.Content.GetType() != typeof (Image))
                    .ToList();
            foreach (var expander in Expanders)
            {
                expander.Expanded += ExpanderOnExpanded;
                expander.Collapsed += ExpanderOnCollapsed;
            }
        }

        private bool mExpanderAlreadyHandled;
        private void ExpanderOnExpanded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (!mExpanderAlreadyHandled)
            {
                var expander = sender as Expander;
                mExpanderAlreadyHandled = true;
                foreach (var exp in Expanders)
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
            foreach (var exp in Expanders)
            {
                exp.IsExpanded = false;
                var row = View.Grid.RowDefinitions[Grid.GetRow(exp)];
                row.Height = GridLength.Auto;
            }
            routedEventArgs.Handled = true;
        }

        public BaseTabView View { get; }

        protected ProgressRing ProgressRing
        {
            get
            {
                var result = Application.Current.MainWindow.FindChildren<ProgressRing>().FirstOrDefault();
                return result;
            }
        }


        public void OpenImage()
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = false,
                InitialDirectory = ViewModel.SettingsModel.StandardPath
            };

            dialog.ShowDialog();
            if (string.IsNullOrEmpty(dialog.FileName))
            {
                return;
            }

            var tmp = Path.ChangeExtension(Path.GetTempFileName(), Path.GetExtension(dialog.FileName));
            File.Copy(dialog.FileName, tmp);
            ViewModel.ImagePath = tmp;
        }

        private void ImageExpanderEvent(object sender, RoutedEventArgs routedEventArgs)
        {
            var expander = sender as Expander;
            ResizeImageExpanderGrid(expander, expander.IsExpanded);
        }

        private void ResizeImageExpanderGrid(UIElement expander, bool expanded)
        {
            var column = View.Grid.ColumnDefinitions[Grid.GetColumn(expander)];
            column.Width = new GridLength(1.0, expanded ? GridUnitType.Star : GridUnitType.Auto);
        }

        public virtual void UnregisterEvents()
        {
            View.ImageExpander.Expanded -= ImageExpanderEvent;
            foreach (var expander in Expanders)
            {
                expander.Expanded -= ExpanderOnExpanded;
                expander.Collapsed -= ExpanderOnCollapsed;
            }
        }

        private void InitializeController()
        {
            RegisterCommands();
        }

        private BaseTabView CreateView(string buttonName, bool textBlockReadOnly, bool resultingTypeVisible)
        {
            return new BaseTabView(buttonName, textBlockReadOnly, resultingTypeVisible);
        }

        protected abstract void RegisterCommands();

        public void SettingsSaved()
        {
            ViewModel.Password = ViewModel.SettingsModel.Password;
            ViewModel.SelectedEncryptionMethod = ViewModel.SettingsModel.SelectedEncryptionMethod;
            ViewModel.SelectedSteganographicMethod = ViewModel.SettingsModel.SelectedSteganographicMethod;
        }
    }
}