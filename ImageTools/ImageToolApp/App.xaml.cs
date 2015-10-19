using System;
using System.Windows;
using System.Windows.Threading;
using ImageToolApp.Controllers;

namespace ImageToolApp
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        [STAThread]
        public static void Main()
        {var application = new App();
            application.InitializeComponent();

            // Global Exception Handler
            Current.DispatcherUnhandledException += UnhandeledExceptionTrapper;
            application.MainWindow = new MainWindow();
            var controller = new MainController(application.MainWindow);

            application.Run();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Current.DispatcherUnhandledException -= UnhandeledExceptionTrapper;

            base.OnExit(e);
            Environment.Exit(0);
        }

        private static void UnhandeledExceptionTrapper(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(Current.MainWindow,
                e.Exception.Message,
                "Fehler",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            e.Handled = true;
        }
    }
}