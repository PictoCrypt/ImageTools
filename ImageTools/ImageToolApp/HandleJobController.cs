using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using ImageToolApp.ViewModels;
using MahApps.Metro.Controls;

namespace ImageToolApp
{
    public static class HandleJobController
    {
        private static readonly Window Window = Application.Current.MainWindow;
        private static readonly MainViewModel MainViewModel = Window.DataContext as MainViewModel;

        private static void WorkDone()
        {
            //Thread.Sleep(TimeSpan.FromSeconds(5));
            Window.Dispatcher.Invoke(() =>
            {
                var progress = Window.FindChildren<ProgressRing>().FirstOrDefault();
                if (progress != null)
                {
                    MainViewModel.ProgressActive = false;
                }
                Window.Cursor = Cursors.Arrow;
            }, DispatcherPriority.Background);
        }

        public static void Progress(Action action)
        {
            var progress = Window.FindChildren<ProgressRing>().FirstOrDefault();
            if (progress != null)
            {
                MainViewModel.ProgressActive = true;
            }
            Window.Cursor = Cursors.Wait;

            var thread = new Thread(() =>
            {
                action.Invoke();
                WorkDone();
            })
            {
                IsBackground = true,
                Name = "EncryptDecryptThread"
            };
            thread.Start();
        }
    }
}