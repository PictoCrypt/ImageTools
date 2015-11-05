using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ImageToolApp.ViewModels;
using MahApps.Metro.Controls;

namespace ImageToolApp
{
    public class HandleJobController : IDisposable
    {
        private static readonly Window Window = Application.Current.MainWindow;
        private readonly MainViewModel mViewModel = Window.DataContext as MainViewModel;

        public void Dispose()
        {
            var progress = Window.FindChildren<ProgressRing>().FirstOrDefault();
            if (progress != null)
            {
                mViewModel.ProgressActive = false;

                //Window.Dispatcher.Invoke((Action)(() =>
                //{
                //    progress.Visibility = Visibility.Collapsed;
                //    progress.IsActive = false;
                //}), DispatcherPriority.Background);
            }
            Window.Cursor = Cursors.Arrow;
        }
        
        public void Progress(Action action)
        {
            var progress = Window.FindChildren<ProgressRing>().FirstOrDefault();
            if (progress != null)
            {
                mViewModel.ProgressActive = true;

                //Window.Dispatcher.Invoke((Action)(() =>
                //{
                //    progress.Visibility = Visibility.Visible;
                //    progress.IsActive = true;
                //}), DispatcherPriority.Background);
            }
            Window.Cursor = Cursors.Wait;

            var worker = new BackgroundWorker
            {
                WorkerReportsProgress = false,
                WorkerSupportsCancellation = false
            };
            worker.DoWork += (sender, args) =>
            {
                action.Invoke();
            };
            worker.RunWorkerCompleted += (sender, args) =>
            {
                if (args.Cancelled == false && args.Error == null)
                {
                    Dispose();
                }
                else
                {
                    throw args.Error;
                }
            };

            worker.RunWorkerAsync();
        }
    }
}